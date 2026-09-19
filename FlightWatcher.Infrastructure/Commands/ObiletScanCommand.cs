using System.Globalization;
using FlightWatcher.Application.Contexts;
using FlightWatcher.Application.Interfaces;
using FlightWatcher.Core.Models;
using FlightWatcher.Core.Models.Providers;
using Microsoft.Playwright;

namespace FlightWatcher.Infrastructure.Commands
{
    public class ObiletScanCommand : IFlightScanCommand
    {
        public async Task Execute(
            string from,
            string to,
            DateOnly departureDay,
            bool isOneWay,
            IList<Passenger> passengers,
            DateOnly? returnDay = null)
        {
            var link = "https://www.obilet.com/ucuslar/";

            var provider = new Obilet
            {
                From = from,
                To = to,
                DepartureDay = departureDay,
                ReturnDay = returnDay,
                IsOneWay = isOneWay,
                Passengers = passengers
            };

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            await using var browserContext = await browser.NewContextAsync();
            var page = await browserContext.NewPageAsync();
            await page.GotoAsync(provider.Url);

            await page.Locator("#origin-input").FillAsync(provider.From);
            await page.Locator("#destination-input").FillAsync(provider.To);

            var originId = (await page
                .Locator("#origin-input + div.display span.id")
                .InnerTextAsync()).Trim();

            var destinationId = (await page
                .Locator("#destination-input + div.display span.id")
                .InnerTextAsync()).Trim();

            link += $"{originId}-{destinationId}/";
            var departureDate = provider.DepartureDay.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            if (provider.IsOneWay)
            {
                await page.Locator("#one-ways").CheckAsync();
                link += departureDate;
            }
            else
            {
                var returnDate = provider.ReturnDay
                    ?? throw new ArgumentException("Gidiş dönüş için dönüş tarihi gereklidir.", nameof(returnDay));

                await page.Locator("#two-ways").CheckAsync();
                link += $"{departureDate}-{returnDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}";
            }

            link += "/" + string.Join("-", provider.GetPassengerParameters()) + "/economy/all";

            await page.GotoAsync(link);

            var journeys = page.Locator("ul#outbound-journeys > li > div.journey.row");
            await journeys.First.Locator(":scope > div.price.col > div.amount.notranslate:not(.close-price) > div.amount-integer").WaitForAsync();
            var culture = CultureInfo.GetCultureInfo("tr-TR");

            foreach (var journey in await journeys.AllAsync())
            {
                var flights = journey.Locator(":scope > ul.flights.col > li.flight");
                var airline = await flights.First.Locator(".airline .name").InnerTextAsync();
                var departure = await flights.First.Locator(".departure").InnerTextAsync();
                var arrival = await flights.Last.Locator(".arrival").InnerTextAsync();
                var originAirport = flights.First.Locator(".flight-origin");
                var destinationAirport = flights.Last.Locator(".flight-arrival");
                var price = await journey.Locator(":scope > div.price.col > div.amount.notranslate:not(.close-price) > div.amount-integer").InnerTextAsync();

                var ticketPrice = int.Parse(price.Trim(), NumberStyles.AllowThousands, culture);

                if (ticketPrice <= FlightWatcherContext.TargetTicket.TargetPrice)
                {
                    FlightWatcherContext.Flights.Add(new Flights
                    {
                        Airline = airline.Trim(),
                        DepartureAirport = (await originAirport.GetAttributeAsync("title"))?.Trim() ?? string.Empty,
                        DepartureAirportCode = (await originAirport.Locator(".airport").InnerTextAsync()).Trim(),
                        ArrivalAirport = (await destinationAirport.GetAttributeAsync("title"))?.Trim() ?? string.Empty,
                        ArrivalAirportCode = (await destinationAirport.Locator(".airport").InnerTextAsync()).Trim(),
                        DepartureTime = TimeOnly.Parse(departure.Trim(), culture),
                        ArrivedTime = TimeOnly.Parse(arrival.Trim(), culture),
                        TicketPrice = ticketPrice
                    });
                }
            }
        }
    }
}
