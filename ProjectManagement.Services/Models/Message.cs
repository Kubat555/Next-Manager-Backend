using MimeKit;


namespace ProjectManagement.Services.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }

    }
}
