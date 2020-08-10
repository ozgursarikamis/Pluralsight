using System;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
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
    }
}
