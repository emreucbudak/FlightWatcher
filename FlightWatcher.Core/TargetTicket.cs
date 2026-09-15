namespace FlightWatcher.Core
{
    public class TargetTicket
    {
        public string Location { get; set; }
        public string Destination {  get; set; }
        public int TargetPrice {  get; set; }
        public DateOnly LandedDay { get; set; }

    }
}
