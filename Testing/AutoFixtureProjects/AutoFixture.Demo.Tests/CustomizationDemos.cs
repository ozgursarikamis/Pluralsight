using System;
using AutoFixture;
using Fixture.Tests.SpecimenBuilders;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class CustomizationDemos
    {
        [Fact]
        public void DateTimeCustomization()
        {
            var fixture = new AutoFixture.Fixture();

            // fixture.Customize(new CurrentDateTimeCustomization());
            fixture.Customizations.Add(new CurrentDateTimeGenerator());

            var date1 = fixture.Create<DateTime>();
            var date2 = fixture.Create<DateTime>();
        }

        [Fact]
        public void CustomizationPipeline()
        {
            var fixture = new AutoFixture.Fixture();
            fixture.Customizations.Add(new AirportCodeStringPropertyGenerator());

            var flight = fixture.Create<FlightDetails>();
            var airport = fixture.Create<Airport>();
        }
    }
}
