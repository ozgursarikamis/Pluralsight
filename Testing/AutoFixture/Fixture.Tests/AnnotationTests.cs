using AutoFixture;
using Xunit;

namespace Fixture.Tests
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