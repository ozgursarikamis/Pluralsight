using AutoFixture;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class CustomTypeDemos
    {
        [Fact]
        public void ManualCreation()
        {
            var sut = new EmailMessageBuffer();
            EmailMessage message = new EmailMessage(
                "sarah@dontcodetired.com", "Hello, How are you?", false);

            message.Subject = "Hi";

            sut.Add(message);

            Assert.Single(sut.Emails);
        }
        [Fact]
        public void AutoCreation()
        {
            var fixture = new AutoFixture.Fixture();
            var sut = new EmailMessageBuffer();

            EmailMessage message = fixture.Create<EmailMessage>();

            sut.Add(message);
            Assert.Single(sut.Emails);
        }
    }
}