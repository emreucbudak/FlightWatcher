using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Commands
{
    public class PegasusScanCommand : IFlightScanCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
