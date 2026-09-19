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
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            await using var browserContext = await browser.NewContextAsync();
            var page = await browserContext.NewPageAsync();
            await page.GotoAsync(provider.Url);
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
