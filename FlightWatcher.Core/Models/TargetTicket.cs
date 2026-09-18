namespace FlightWatcher.Core.Models
{
    public class TargetTicket
    {
        public string Location { get; set; }
        public string Destination {  get; set; }
        public int TargetPrice {  get; set; }
        public DateOnly LandedDay { get; set; }

        public IList<Passenger> Passengers { get; set; } = new List<Passenger>();

    }
}
