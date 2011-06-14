
namespace PAZ.Model
{
    public class Email
    {
        public Email(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }

        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set;  }
    }
}
