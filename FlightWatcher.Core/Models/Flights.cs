namespace FlightWatcher.Core.Models
{
    public class Flights
    {
        public string Airline {  get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivedTime {  get; set; }
        public int TicketPrice { get; set; }
        public string DepartureAirport { get; set; } = string.Empty;
        public string DepartureAirportCode { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public string ArrivalAirportCode { get; set; } = string.Empty;
    }
}
