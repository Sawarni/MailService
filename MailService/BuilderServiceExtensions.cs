using MailService.Configurations;
using MailService.Middlewares;
using MassTransit;
using NLog.Extensions.Logging;
using Shared;

namespace MailService
{
    public static class BuilderServiceExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddLogging( logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel( LogLevel.Debug );
            });

            builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

            builder.Services.AddOptions<AppConfigurations>().Bind(builder.Configuration.GetSection(AppConfigurations.Key));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            var mqSettings = builder.Configuration.GetSection(MqSettings.Key).Get<MqSettings>();
            if(mqSettings == null)
            {
                throw new Exception("Mq Setting is not configured");
            }
            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host(new Uri(mqSettings.Url), h =>
                    {
                        h.Username(mqSettings.UserName);
                        h.Password(mqSettings.Password);
                    });
                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

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
