namespace FlightWatcher.Core.Models.Providers
{
    public class AJet
    {
        public string Url { get; set; } = "https://ajet.com/tr";

        public string From { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public DateOnly DepartureDay { get; set; }

        public DateOnly? ReturnDay { get; set; }

        public bool IsOneWay { get; set; }

        public IList<Passenger> Passengers { get; set; } = new List<Passenger>();
    }
}
