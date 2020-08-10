using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class StringDemos
    {
        [Fact]
        public void BasicStrings()
        {
            // arr
            var fixture = new AutoFixture.Fixture();
            var sut = new NameJoiner();


            var firstName = fixture.Create("First_");
            var lastName = fixture.Create("Last_");

            // act
            var result = sut.Join(firstName, lastName);

            // ass

            Assert.Equal(firstName +  ' ' + lastName, result );
        }
    }
}