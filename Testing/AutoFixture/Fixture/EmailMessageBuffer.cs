using System.Collections.Generic;

namespace AutoFixtureDemo
{
    public class EmailMessageBuffer
    {
        public List<EmailMessage> Emails { get; private set; } = new List<EmailMessage>();
        
        public void SendAll()
        {
            foreach (var email  in Emails)
            {
                // Send email
            }
        }

        public void Add(EmailMessage message)
        {
            Emails.Add(message);
        }
    }
}
