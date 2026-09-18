using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Commands
{
    public class TurnaScanCommand : IFlightScanCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
