using FlightWatcher.Core.Enums;

namespace FlightWatcher.Core.Models.Providers
{
    public class Obilet
    {
        public string Url { get; set; } = "https://www.obilet.com/ucak-bileti";

        public string From { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public DateOnly DepartureDay { get; set; }

        public DateOnly? ReturnDay { get; set; }

        public bool IsOneWay { get; set; }

        public IList<Passenger> Passengers { get; set; } = new List<Passenger>();

        public string[] GetPassengerParameters()
        {
            var parameters = new string[Passengers.Count];

            for (var i = 0; i < Passengers.Count; i++)
            {
                var passenger = Passengers[i];
                if (passenger.PersonType == PersonType.Adult)
                    parameters[i] = passenger.PersonCount + "a";
                else if (passenger.PersonType == PersonType.Student)
                    parameters[i] = passenger.PersonCount + "s";
                else
                    parameters[i] = passenger.PersonCount + "c";
            }

            return parameters;
        }
    }
}
