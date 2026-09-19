using FlightWatcher.Core.Models;

namespace FlightWatcher.Application.Contexts
{
    public static class FlightWatcherContext
    {
        public static IList<Flights> Flights { get; } = new List<Flights>();

        public static TargetTicket TargetTicket { get; set; } = new TargetTicket();

        public static DateOnly LandedDay { get; set; }
    }
}
