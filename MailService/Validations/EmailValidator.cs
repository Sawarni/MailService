using FluentValidation;
using Shared.Contracts;

namespace MailService.Validations
{
    public class EmailValidator : AbstractValidator<EmailMessage>
    {
        public EmailValidator() 
        {
            RuleFor(x => x.To).NotEmpty().EmailAddress();
            RuleFor(x => x.From).NotEmpty().EmailAddress();            
        }
    }
}
