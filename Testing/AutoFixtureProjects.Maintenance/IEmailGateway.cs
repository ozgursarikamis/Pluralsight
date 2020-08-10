using System.Diagnostics;

namespace AutoFixtureProjects.Maintenance
{
    public interface IEmailGateway
    {
        void Send(EmailMessage message);
    }

    public class EmailGateway : IEmailGateway
    {
        public void Send(EmailMessage message)
        {
            // simulate sending email
            Debug.WriteLine("Sending email to: " + message.ToAddress);
        }
    }
}