using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoFixtureProjects.Maintenance
{
    public class EmailMessageBuffer
    {

        private readonly List<EmailMessage> _emails = new List<EmailMessage>();
        public EmailMessageBuffer(IEmailGateway emailGateway)
        {
            EmailGateway = emailGateway;
        }

        public IEmailGateway EmailGateway { get; }
        public int UnsentMessagesCount => _emails.Count;

        public void Add(EmailMessage message)
        {
            _emails.Add(message);
        }

        public void SendAll()
        {
            for (var i = 0; i < _emails.Count; i++)
            {
                var email = _emails[i];

                Send(email);
                _emails.Remove(email);
            }
        }

        public void SendLimited(int maximumMessagesToSend)
        {
            var limitedBatchOfMessages = _emails.Take(maximumMessagesToSend).ToArray();

            foreach (var email in limitedBatchOfMessages)
            {
                Send(email);
                _emails.Remove(email);
            }
        }

        private void Send(EmailMessage email)
        {
            EmailGateway.Send(email);
        }
    }
}
