using Shared.Contracts;

namespace SmtpService.Services
{
    public interface INotificationService
    {
        Task<(bool IsMailSent, bool IsMailSaved, string errorMessage)> SendAndSaveMail(IEmailMessage message);
    }
}