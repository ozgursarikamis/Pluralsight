using System.Collections.Generic;
using System.Net.Mail;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class SequenceDemos
    {
        [Fact]
        public void SequenceOfStrings()
        {
            var fixture = new AutoFixture.Fixture();

            IEnumerable<string> messages = fixture.CreateMany<string>(6);

        }
    }
}
