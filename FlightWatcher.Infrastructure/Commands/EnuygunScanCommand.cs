using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Commands
{
    public class EnuygunScanCommand : IFlightScanCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
