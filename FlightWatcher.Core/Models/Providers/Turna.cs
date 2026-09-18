namespace FlightWatcher.Core.Models.Providers
{
    public class Turna
    {
        public string From { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public DateOnly DepartureDay { get; set; }

        public bool IsOneWay { get; set; }
    }
}
