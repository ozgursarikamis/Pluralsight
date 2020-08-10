using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class GuidEnumDemos
    {
        [Fact]
        public void Guid()
        {
            var fixture = new AutoFixture.Fixture();
            var sut = new EmailMessage(
                fixture.Create<string>(), 
                fixture.Create<string>(), 
                fixture.Create<bool>());

            sut.Id = fixture.Create<Guid>();


        }

        [Fact]
        public void Enums()
        {
            var fixture = new AutoFixture.Fixture();

            var sut = new EmailMessage(
                fixture.Create<string>(),
                fixture.Create<string>(),
                fixture.Create<bool>());

            sut.Id = fixture.Create<Guid>();
            sut.MessageType = fixture.Create<EmailMessageType>();


        }
    }
}