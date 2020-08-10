using AutoFixture;
using Xunit;

namespace AutoFixtureDemo.Tests
{
    public class AnnotationTests
    {
        [Fact]
        public void BasicString()
        {
            var fixture = new AutoFixture.Fixture();
            var player = fixture.Create<PlayerCharacter>();
        }
    }
}