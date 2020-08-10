using System;

namespace AutoFixtureProjects.Maintenance
{
    public class FlightDetails
    {
        public FlightDetails(string departureAirportCode, string arrivalAirportCode)
        {
            DepartureAirportCode = departureAirportCode;
            ArrivalAirportCode = arrivalAirportCode;
        }

        public string DepartureAirportCode { get; }
        public string ArrivalAirportCode { get; }
        public TimeSpan FlightDuration { get; set; }
        public string AirlineName { get; set; }
    }
}