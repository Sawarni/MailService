using Shared.Contracts;

namespace SmtpService.Services
{
    public interface IEmailService
    {
        (bool isSuccess, string errorMessage) Send(IEmailMessage emailMessage);
    }
}