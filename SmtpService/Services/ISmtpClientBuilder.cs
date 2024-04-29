using System.Net.Mail;

namespace SmtpService.Services
{
    public interface ISmtpClientBuilder
    {
        SmtpClient Build();
    }
}