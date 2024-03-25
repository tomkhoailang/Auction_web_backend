using MimeKit;

namespace Chat.Service.Models
{
    public class MessageEmail
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MessageEmail(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Email", x)));
            Subject = subject;
            Content = content;
        }

    }
}
