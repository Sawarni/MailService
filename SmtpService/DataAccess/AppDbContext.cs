using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmtpService.DataAccess.Model;
using SmtpService.DataAccess.ModelConfiguration;

namespace SmtpService.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option, ILogger<AppDbContext> logger):base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new MailMessageConfiguration());
        }

        public DbSet<Mail> Mails { get; set; }

    }
}
