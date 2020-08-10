using System.Net.Mail;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class EmailAddressDemos
    {
        [Fact]
        public void Email()
        {
            var fixture = new AutoFixture.Fixture();
            //string localPart = fixture.Create<EmailAddressLocalPart>().LocalPart;
            //string domain = fixture.Create<DomainName>().Domain;
            //string fullAddress = $"{localPart}@{domain}";
             
            var sut = new EmailMessage(
                fixture.Create<MailAddress>().Address,
                fixture.Create<string>(),
                fixture.Create<bool>()
            );


        }
    }
}