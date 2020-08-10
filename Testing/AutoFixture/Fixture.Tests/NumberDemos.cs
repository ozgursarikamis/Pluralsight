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

        [Fact]
        public void Decimals()
        {
            // arr
            var fixture = new AutoFixture.Fixture();
            var sut = new DecimalCalculator();

            decimal num = fixture.Create<decimal>();

            // act
            sut.Add(num);

            // ass
            Assert.Equal(num, sut.Value);
        }
    }
}