using System.Globalization;
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

            link += "/" + string.Join("-", provider.GetPassengerParameters());
        }
    }
}
