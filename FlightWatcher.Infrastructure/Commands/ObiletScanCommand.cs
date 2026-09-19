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
            IList<Passenger> passengers)
        {
            var provider = new Obilet
            {
                From = from,
                To = to,
                DepartureDay = departureDay,
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
        }
    }
}
