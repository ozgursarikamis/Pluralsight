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

        [Fact]
        public void CustomCreationFunction()
        {
            var fixture = new AutoFixture.Fixture();

            fixture.Register(() => DateTime.Now.Ticks.ToString());

            var string1 = fixture.Create<string>();
            var string2 = fixture.Create<string>();
        }

        [Fact]
        public void FreezingValues()
        {
            var fixture = new AutoFixture.Fixture();
            
            var id = fixture.Freeze<int>();
            var customerName = fixture.Freeze<string>();

            var sut = fixture.Create<Order>();

            Assert.Equal(id + "-" + customerName, sut.ToString());
        }

        [Fact]
        public void OmitSettingSpecificProperties()
        {
            var fixture = new AutoFixture.Fixture();
            var flight = fixture.Build<FlightDetails>()
                .OmitAutoProperties()
                .Create();
        }

        [Fact]
        public void CustomizedBuilding()
        {
            var fixture = new AutoFixture.Fixture();
            var flight = fixture.Build<FlightDetails>()
                .With(x => x.ArrivalAirportCode, "LAX")
                .With(x => x.DepartureAirportCode, "LHR")
                .Create();
        }

        [Fact]
        public void CustomizedBuildingWithActions()
        {
            var fixture = new AutoFixture.Fixture();
            var flight = fixture.Build<FlightDetails>()
                .With(x => x.ArrivalAirportCode, "LAX")
                .With(x => x.DepartureAirportCode, "LHR")
                .Without(x => x.MealOptions)
                .Do(x => x.MealOptions.Add("Chicken"))
                .Do(x => x.MealOptions.Add("Fish"))
                .Create();
        }

        [Fact]
        public void CustomizedBuildingForAllTypesInFixture()
        {
            var fixture = new AutoFixture.Fixture();

            fixture.Customize<FlightDetails>(fd =>
                fd.With(x => x.DepartureAirportCode, "LHR")
                    .With(x => x.ArrivalAirportCode, "LAX")
                    .With(x => x.AirlineName, "Fly Fly Premium Air")
                    .Without(x => x.MealOptions)
                    .Do(x => x.MealOptions.Add("Chicken"))
                    .Do(x => x.MealOptions.Add("Fish"))
            );

            var flight1 = fixture.Create<FlightDetails>();
            var flight2 = fixture.Create<FlightDetails>();
        }
    }
}