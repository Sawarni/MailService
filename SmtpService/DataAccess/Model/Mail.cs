using Shared.Contracts;

namespace SmtpService.DataAccess.Model
{
    public class Mail : EmailMessage
    {
        public Guid Id { get; set; }

        public bool SendStatus { get; set; }

        public DateTime? SentDateTime { get; set; }

        public string SmtpResponse { get; set; }

        public Mail()
        {
                
        }
        public Mail(IEmailMessage message)
        {
            Id = Guid.Empty;
            SendStatus = false;
            SentDateTime = default;
            SmtpResponse = string.Empty;
            From = message.From;
            To = message.To;
            Subject = message.Subject;
            Body = message.Body;
        }
    }
}
