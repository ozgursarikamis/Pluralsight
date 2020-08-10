using System;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class DateAndTimeDemos
    {
        [Fact]
        public void DateTimes()
        {
            var fixture = new AutoFixture.Fixture();
            //DateTime logTime = new DateTime(2020, 1, 21);
            var logTime = fixture.Create<DateTime>();

            LogMessage result = LogMessageCreator.Create("some log messsage", logTime);

            Assert.Equal(2020, result.Year);
        }

        [Fact]
        public void TimeSpans()
        {
            var fixture = new AutoFixture.Fixture();
            TimeSpan timeSpan = fixture.Create<TimeSpan>();


        }
    }
}