using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmtpService.DataAccess.Model;

namespace SmtpService.DataAccess.ModelConfiguration
{
    public class MailMessageConfiguration : IEntityTypeConfiguration<Mail>
    {
        public void Configure(EntityTypeBuilder<Mail> builder)
        {
            builder.HasKey(e=> e.Id);
            builder.Property(e=> e.Id).HasColumnName("Id").IsRequired();
            builder.Property(e=> e.From).HasColumnName("FromId").IsRequired();
            builder.Property(e => e.To).HasColumnName("ToId").IsRequired();
            builder.Property(e => e.Subject).HasColumnName("MailSubject");
            builder.Property(e => e.Body).HasColumnName("MailBody");
            builder.Property(e => e.SendStatus).HasColumnName("SendStatus");
            builder.Property(e=> e.SentDateTime).HasColumnName("SentDateTime");
            builder.Property(e => e.SmtpResponse).HasColumnName("SmtpResponse");
            builder.ToTable("Mail","dbo");


        }
    }
}
