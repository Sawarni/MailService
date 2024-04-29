using Shared.Contracts;
using System.Net.Mail;

namespace SmtpService.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger, ISmtpClientBuilder smtpClientBuilder)
        {
            _logger = logger;
            _smtpClient = smtpClientBuilder.Build();
        }

        public (bool isSuccess, string errorMessage) Send(IEmailMessage emailMessage)
        {
            _logger.LogInformation("Sending mail started");

            try
            {
                var mailMessage = new MailMessage(emailMessage.From, emailMessage.To, emailMessage.Subject, emailMessage.Body);
                _smtpClient.Send(mailMessage);
                _logger.LogInformation("Sending mail finished");
                return (true, string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return (false, e.Message);
            }
        }
    }
}
