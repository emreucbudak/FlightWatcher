namespace FlightWatcher.Core.Models.Providers
{
    public class Enuygun
    {
        public string Url { get; set; } = "https://www.enuygun.com/ucak-bileti/?ref=logo";

        public string From { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public DateOnly DepartureDay { get; set; }

        public bool IsOneWay { get; set; }

        public IList<Passenger> Passengers { get; set; } = new List<Passenger>();
    }
}
