using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Commands
{
    public class AJetScanCommand : IFlightScanCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
