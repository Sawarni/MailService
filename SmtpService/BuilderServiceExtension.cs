using MassTransit;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Shared;
using SmtpService.Configurations;
using SmtpService.DataAccess;
using SmtpService.DataAccess.Repository;
using SmtpService.Middlewares;
using SmtpService.Services;
using SmtpService.WorkerService;

namespace SmtpService
{
    public static class BuilderServiceExtension
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

            builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("maildb"));
            builder.Services.AddScoped<IMailRepository, MailRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISmtpClientBuilder, SmtpClientBuilder>();
            builder.Services.AddOptions<SmtpServerConfiguration>().Bind(builder.Configuration.GetSection(SmtpServerConfiguration.Key));
            builder.Services.AddOptions<MailRateConfiguration>().Bind(builder.Configuration.GetSection(MailRateConfiguration.Key));

            var mqSettings = builder.Configuration.GetSection(MqSettings.Key).Get<MqSettings>();
            var mailRate = builder.Configuration.GetSection(MailRateConfiguration.Key).Get<MailRateConfiguration>()
                ?? new MailRateConfiguration { AllowedMailQuota = 2, InDuration = 60 };
            ;

            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<MailConsumer>(cfg =>
                {
                    cfg.UseRateLimit(mailRate.AllowedMailQuota, TimeSpan.FromSeconds(mailRate.InDuration));
                });
                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host(mqSettings.Url, h =>
                    {
                        h.Username(mqSettings.UserName);
                        h.Password(mqSettings.Password);
                    });
              
                    busFactoryConfigurator.ConfigureEndpoints(context);
                   

                });

            }

            );

            builder.Services.AddHealthChecks();

            return builder;
        }

        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/healthz");

            return app;
        }
    }
}
