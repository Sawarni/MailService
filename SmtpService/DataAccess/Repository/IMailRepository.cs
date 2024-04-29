using SmtpService.DataAccess.Model;
using System.Linq.Expressions;

namespace SmtpService.DataAccess.Repository
{
    public interface IMailRepository
    {
        Task<Mail> AddMail(Mail mail);
        Task<List<Mail>> GetAll();
        Task<List<Mail>> GetByCondition(Expression<Func<Mail, bool>> expression);
        Task<Mail?> GetById(Guid id);
        Task<bool> MarkMailSent(Guid mailId);
    }
}