using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace AutoFixtureProjects.Maintenance.Tests
{
    public class EmailMessageBufferShould
    {
        [Fact]
        public void SendEmailToGateway_Manual_Moq()
        {
            var fixture = new Fixture();

            var mockGateway = new Mock<IEmailGateway>();
            var sut = new EmailMessageBuffer(mockGateway.Object);

            sut.Add(fixture.Create<EmailMessage>());

            sut.SendAll();

            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Fact]
        public void SendEmailToGateway_AutoMoq()
        {
            var fixture = new Fixture();
            var mockGateway = fixture.Freeze<Mock<IEmailGateway>>();

            fixture.Customize(new AutoMoqCustomization());

            var sut = fixture.Create<EmailMessageBuffer>();
            sut.Add(fixture.Create<EmailMessage>());

            sut.SendAll();

            // assert
            // no reference to the mock IEmailGateway
            // that was automatically provided
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
    }
}
