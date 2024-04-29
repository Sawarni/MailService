using Shared.Contracts;
using SmtpService.DataAccess.Repository;
using SmtpService.Mapper;

namespace SmtpService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMailRepository _mailRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IMailRepository mailRepository, IEmailService emailService, ILogger<NotificationService> logger)
        {
            _mailRepository = mailRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<(bool IsMailSent, bool IsMailSaved, string errorMessage)> SendAndSaveMail(IEmailMessage message)
        {
            _logger.LogInformation("Sending mail");
            var sendResult = _emailService.Send(message);
            _logger.LogInformation("Sending mail - completed");
            var mailMessage = message.ToMail();
            mailMessage.SendStatus = sendResult.isSuccess;
            mailMessage.SmtpResponse = sendResult.errorMessage;
            mailMessage.SentDateTime = DateTime.UtcNow;
            _logger.LogInformation("Saving mail");
            var savedEntity = await _mailRepository.AddMail(mailMessage).ConfigureAwait(false);
            _logger.LogInformation("Saving mail-- Completed");
            return (sendResult.isSuccess, savedEntity.Id != Guid.Empty, sendResult.errorMessage);
        }
    }
}
