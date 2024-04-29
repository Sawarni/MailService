using Microsoft.Extensions.Options;
using SmtpService.Configurations;
using System.Net;
using System.Net.Mail;

namespace SmtpService.Services
{
    public class SmtpClientBuilder : ISmtpClientBuilder
    {

        private SmtpClient _smtpClient;
        private readonly SmtpServerConfiguration _smtpServerConfiguration;
        private readonly ILogger<SmtpClientBuilder> _logger;
        public SmtpClientBuilder(IOptions<SmtpServerConfiguration> smtpServerConfigurationOption, ILogger<SmtpClientBuilder> logger)
        {
            _smtpServerConfiguration = smtpServerConfigurationOption.Value;
            _logger = logger;
            _smtpClient = new SmtpClient();
        }

        public SmtpClient Build()
        {
            _logger.LogInformation("Initializing the smtp based on configurations.");
            if (_smtpServerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(SmtpServerConfiguration));
            }

            AddHostAndPort(_smtpServerConfiguration.SmtpHost, _smtpServerConfiguration.SmtpPort)
            .AddCredetials(_smtpServerConfiguration.IsAuthenticatedSmtp, _smtpServerConfiguration.UseDefaultCredentials, _smtpServerConfiguration.SmtpUsername, _smtpServerConfiguration.SmtpPassword)
            .AddDeliveryOptions(_smtpServerConfiguration.IsDeliverToFolder, _smtpServerConfiguration.FolderDirectoryLocation);
            _logger.LogInformation("Initialized smtp based on configurations.");
            return _smtpClient;

        }

        private SmtpClientBuilder AddHostAndPort(string host, int port)
        {
            _smtpClient.Host = host;
            if (port > 0)
            {
                _smtpClient.Port = port;
            }
            return this;
        }

        private SmtpClientBuilder AddCredetials(bool isAuthenticated, bool useDefaultCredentials, string userName, string password)
        {
            if (isAuthenticated)
            {
                _smtpClient.UseDefaultCredentials = useDefaultCredentials;
                _smtpClient.Credentials = useDefaultCredentials ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(userName, password);
            }

            return this;
        }

        private SmtpClientBuilder AddDeliveryOptions(bool isDeliverToFolder, string folderDirectory)
        {
            if (isDeliverToFolder)
            {
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                _smtpClient.PickupDirectoryLocation = Path.Combine(Environment.CurrentDirectory, folderDirectory ?? string.Empty);
                if(!Directory.Exists(_smtpClient.PickupDirectoryLocation))
                {
                    Directory.CreateDirectory(_smtpClient.PickupDirectoryLocation);
                }

            }
            return this;
        }


    }
}
