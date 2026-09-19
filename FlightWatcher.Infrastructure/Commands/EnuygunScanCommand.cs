using System.Globalization;
using FlightWatcher.Application.Contexts;
using FlightWatcher.Application.Interfaces;
using FlightWatcher.Core.Models;
using FlightWatcher.Core.Models.Providers;
using Microsoft.Playwright;

namespace FlightWatcher.Infrastructure.Commands
{
    public class EnuygunScanCommand : IFlightScanCommand
    {
        public async Task Execute(
            string from,
            string to,
            DateOnly departureDay,
            bool isOneWay,
            IList<Passenger> passengers,
            DateOnly? returnDay = null)
        {
            var ticketUrl = "https://www.enuygun.com/ucak-bileti/arama";

            var provider = new Enuygun
            {
                From = from,
                To = to,
                DepartureDay = departureDay,
                ReturnDay = returnDay,
                IsOneWay = isOneWay,
                Passengers = passengers
            };

            var (originAirport, originAirportCode) = ParseAirport(provider.From);
            var (destinationAirport, destinationAirportCode) = ParseAirport(provider.To);
            ticketUrl += $"/{originAirport}-{destinationAirport}-{originAirportCode}-{destinationAirportCode}/";
            ticketUrl += $"?gidis={provider.DepartureDay.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";

            if (!provider.IsOneWay)
            {
                var returnDate = provider.ReturnDay
                    ?? throw new ArgumentException("Gidiş dönüş için dönüş tarihi gereklidir.", nameof(returnDay));
                ticketUrl += $"&donus={returnDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";
            }

            foreach (var parameter in provider.GetPassengerParameters())
            {
                ticketUrl += $"&{parameter.Key}={parameter.Value.ToString(CultureInfo.InvariantCulture)}";
            }

            ticketUrl += "&sinif=ekonomi";
            ticketUrl += "&save=1&ref=ft-homepage&geotrip=domestic&trip=domestic&ref=ft-homepage";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            await using var browserContext = await browser.NewContextAsync();
            var page = await browserContext.NewPageAsync();
            await page.GotoAsync(ticketUrl);
            var cards = page.Locator(".flight-item .flight-summary");
            await cards.First.Locator(".flight-summary-price .money-int").WaitForAsync();
            var culture = CultureInfo.GetCultureInfo("tr-TR");
            var originAirportName = provider.From.Split(" - ", 2, StringSplitOptions.TrimEntries)[1];
            var destinationAirportName = provider.To.Split(" - ", 2, StringSplitOptions.TrimEntries)[1];

            foreach (var card in await cards.AllAsync())
            {
                var airports = card.Locator(".summary-airports .itemAirport");
                var departureCode = (await airports.First.InnerTextAsync()).Trim();
                var arrivalCode = (await airports.Last.InnerTextAsync()).Trim();

                if (!departureCode.Equals(originAirportCode, StringComparison.OrdinalIgnoreCase)
                    || !arrivalCode.Equals(destinationAirportCode, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var price = await card.Locator(".flight-summary-price .money-int").InnerTextAsync();
                var ticketPrice = int.Parse(price.Trim(), NumberStyles.AllowThousands, culture);
                if (ticketPrice > FlightWatcherContext.TargetTicket.TargetPrice)
                {
                    continue;
                }

                var airline = await card.Locator(".summary-marketing-airlines").InnerTextAsync();
                var departure = await card.GetByTestId("departureTime").InnerTextAsync();
                var arrival = await card.GetByTestId("arrivalTime").InnerTextAsync();

                FlightWatcherContext.Flights.Add(new Flights
                {
                    Airline = airline.Trim(),
                    DepartureAirport = originAirportName,
                    DepartureAirportCode = departureCode,
                    ArrivalAirport = destinationAirportName,
                    ArrivalAirportCode = arrivalCode,
                    DepartureTime = TimeOnly.ParseExact(departure.Trim(), "HH:mm", CultureInfo.InvariantCulture),
                    ArrivedTime = TimeOnly.ParseExact(arrival.Trim(), "HH:mm", CultureInfo.InvariantCulture),
                    TicketPrice = ticketPrice
                });
            }
        }

        private static (string Airport, string AirportCode) ParseAirport(string airportText)
        {
            var parts = airportText.Split(" - ", 2, StringSplitOptions.TrimEntries);
            var locationParts = parts[0].Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2 || locationParts.Length < 3 || string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new ArgumentException("Havalimanı metni 'Şehir, Türkiye KOD - Havalimanı' biçiminde olmalıdır.", nameof(airportText));
            }

            var airportCode = locationParts[2].ToLowerInvariant();
            var airport = airportCode == "gny"
                ? "sanliurfa-guney-anadolu-havalimani"
                : string.Join("-", parts[1]
                    .ToLowerInvariant()
                    .Replace('ı', 'i')
                    .Replace('İ', 'i')
                    .Replace('ş', 's')
                    .Replace('ğ', 'g')
                    .Replace('ü', 'u')
                    .Replace('ö', 'o')
                    .Replace('ç', 'c')
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

            return (airport, airportCode);
        }
    }
}
