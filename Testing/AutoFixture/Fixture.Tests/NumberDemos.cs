using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class NumberDemos
    {
        [Fact]
        public void Ints()
        {
            // arrange
            var sut = new IntCalculator();
            var fixture = new AutoFixture.Fixture();

            // act
            sut.Subtract(fixture.Create<int>());

            // assert
            Assert.True(sut.Value < 0);
        }
    }
}