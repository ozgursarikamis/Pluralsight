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
    }
}