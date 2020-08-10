using AutoFixture;
using Xunit;

namespace AutoFixtureProjects.Maintenance.Tests
{
    public class BookingShould
    {
        [Fact]
        public void CalculateTotalFlightTime()
        {
            var fixture = new Fixture();
            fixture.Inject(new AirportCode("LHR"));
            var sut = fixture.Create<Booking>();

            // etc.
        }
    }
}