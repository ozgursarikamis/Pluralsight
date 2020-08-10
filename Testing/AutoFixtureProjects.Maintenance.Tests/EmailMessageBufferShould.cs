using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
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

        [Theory]
        [AutoMoqData]
        public void SendEmailToGateway_AutoMoq(
            EmailMessage message,
            [Frozen] Mock<IEmailGateway> mockGateway,
            EmailMessageBuffer sut)
        {
            sut.Add(message);
            sut.SendAll();

            // assert
            // no reference to the mock IEmailGateway
            // that was automatically provided
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
    }
}
