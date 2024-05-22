using FluentValidation;
using MailService.Configurations;
using MailService.Validations;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Contracts;
using static MassTransit.ValidationResultExtensions;

namespace MailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<MailController> _logger;
        private readonly IValidator<EmailMessage> _validator;
        private readonly AppConfigurations _appConfigurations;
        public MailController(IPublishEndpoint publishEndpoint, ILogger<MailController> logger, 
            IOptionsSnapshot<AppConfigurations> appConfigurationsSnapshot,
            IValidator<EmailMessage> validator)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _validator = validator;
            _appConfigurations = appConfigurationsSnapshot.Value;
        }

        [HttpPost]

        public async Task<IResult> Notify(EmailMessage message)
        {
            if (message == null)
            {
                return Results.BadRequest();
            }

            var validationResult =  await _validator.ValidateAsync(message);
            if(!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            try
            {
                await _publishEndpoint.Publish<IEmailMessage>(message).WaitAsync(TimeSpan.FromSeconds(_appConfigurations.QueueConnectionTimeOut));

            }
            catch (TimeoutException e)
            {
                _logger.LogError("Unable to publish message to queue in {0} seconds due to error - {1} ", _appConfigurations.QueueConnectionTimeOut, e.Message);
                throw;
            }
            return Results.Accepted();
        }

        [HttpPost]
        [Route("multiple")]
        public async Task<IActionResult> Notify(List<EmailMessage> messages)
        {
            if (messages == null || messages.Count <= 0)
            {
                return BadRequest();
            }
            try
            {
                foreach (var message in messages)
                {
                    await _publishEndpoint.Publish<IEmailMessage>(message).WaitAsync(TimeSpan.FromSeconds(_appConfigurations.QueueConnectionTimeOut));
                    Task.Delay(1000).Wait();
                }

            }
            catch (TimeoutException e)
            {
                _logger.LogError("Unable to publish message to queue in {0} seconds due to error - {1} ", _appConfigurations.QueueConnectionTimeOut, e.Message);
                throw;
            }
            return Accepted();
        }

    }
}
