using FlightWatcher.Core.Models;

namespace FlightWatcher.Application.Interfaces
{
    public interface IFlightScanCommand
    {
        Task Execute(
            string from,
            string to,
            DateOnly departureDay,
            bool isOneWay,
            IList<Passenger> passengers);
    }
}
