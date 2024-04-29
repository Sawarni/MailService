using MassTransit;
using Shared.Contracts;
using SmtpService.Services;

namespace SmtpService.WorkerService
{
    public class MailConsumer : IConsumer<IEmailMessage>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MailConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Consume(ConsumeContext<IEmailMessage> context)
        {
            IEmailMessage message = context.Message;
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var mailService = scope.ServiceProvider.GetService<INotificationService>();
            if (mailService != null)
            {
                await mailService.SendAndSaveMail(message);
            }
        }
    }
}
