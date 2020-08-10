using System;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class CustomizeFixtureDemos
    {

        [Fact]
        public void Error()
        {
            var fixture = new AutoFixture.Fixture();
            var timespan = new TimeSpan(3, 2, 1, 0);
            
            fixture.Inject("LHR");
            fixture.Inject(timespan);
            var flight = fixture.Create<FlightDetails>();
        }
        [Fact]
        public void SettingValuesForCustomType()
        {
            var fixture = new AutoFixture.Fixture();
            fixture.Inject(new FlightDetails
            {
                DepartureAirportCode = "PER",
                ArrivalAirportCode = "LHR",
                FlightDuration = TimeSpan.FromHours(10),
                AirlineName = "Awesome Aero"
            });
            
            var flight1 = fixture.Create<FlightDetails>();
            var flight2 = fixture.Create<FlightDetails>();
        }
    }
}