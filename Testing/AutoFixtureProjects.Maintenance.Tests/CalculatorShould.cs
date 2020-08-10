using AutoFixture.Xunit2;
using Xunit;

namespace AutoFixtureProjects.Maintenance.Tests
{
    public class CalculatorShould
    {
        [Theory]
        [InlineData(1, 2)] // AddTwoPositiveNumbers
        [InlineData(0, 2)] // AddZeroAndPositiveNumber
        [InlineData(-5, 1)] // AddNegativeAndPositiveNumber
        public void Add(int a, int b)
        {
            var sut = new Calculator();
            sut.Add(a);
            sut.Add(b);

            Assert.Equal(a + b, sut.Value);
        }

        [Theory]
        [AutoData] // anonymous test data
        public void AddTwoPositiveNumbers(int a, int b, Calculator sut)
        { 
            sut.Add(a);
            sut.Add(b);

            Assert.Equal(a + b, sut.Value);
        }


        [Fact]
        public void AddZeroAndPositiveNumber()
        {
            var sut = new Calculator();

            sut.Add(0);
            sut.Add(2);

            Assert.Equal(2, sut.Value);
        }


        [Fact]
        public void AddNegativeAndPositiveNumber()
        {
            var sut = new Calculator();

            sut.Add(-5);
            sut.Add(1);

            Assert.Equal(-4, sut.Value);
        } 
    }
}