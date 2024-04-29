using Microsoft.EntityFrameworkCore;
using SmtpService.DataAccess.Model;
using System.Linq.Expressions;


namespace SmtpService.DataAccess.Repository
{
    public class MailRepository : IMailRepository
    {
        private readonly AppDbContext _appDbContext;

        public MailRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IQueryable<Mail> GetMessagesWhere(Expression<Func<Mail, bool>> expression)
        {
            return _appDbContext.Mails.Where(expression);
        }

        public Task<Mail?> GetById(Guid id)
        {
            return GetMessagesWhere(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Mail>> GetAll()
        {
            return _appDbContext.Mails.ToListAsync();
        }

        public Task<List<Mail>> GetByCondition(Expression<Func<Mail, bool>> expression)
        {
            return GetMessagesWhere(expression).ToListAsync();
        }

        public async Task<bool> MarkMailSent(Guid mailId)
        {
            var mailMessage = await GetById(mailId).ConfigureAwait(false);
            if (mailMessage != null)
            {
                mailMessage.SentDateTime = DateTime.UtcNow;
                mailMessage.SendStatus = true;
                mailMessage.SmtpResponse = "Sent manually";
                _appDbContext.Mails.Attach(mailMessage);
                await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;

        }

        public async Task<Mail> AddMail(Mail mail)
        {
            if (mail == null)
                throw new ArgumentNullException(nameof(mail));

            if (mail.Id == Guid.Empty)
            {
                mail.Id = Guid.NewGuid();
            }

            _appDbContext.Mails.Add(mail);
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
            return mail;

        }


    }
}
