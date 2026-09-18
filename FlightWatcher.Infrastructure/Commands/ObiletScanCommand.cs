using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Commands
{
    public class ObiletScanCommand : IFlightScanCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
