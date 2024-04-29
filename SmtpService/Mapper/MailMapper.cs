using Shared.Contracts;
using SmtpService.DataAccess.Model;

namespace SmtpService.Mapper
{
    public static class MailMapper
    {
        public static Mail ToMail(this IEmailMessage message)
        {
            return new Mail(message);
           
        }
    }
}
