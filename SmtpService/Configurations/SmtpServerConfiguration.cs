namespace SmtpService.Configurations
{
    public class SmtpServerConfiguration
    {
        public const string Key = nameof(SmtpServerConfiguration);

        public string SmtpHost { get; set; } = string.Empty;

        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set;} = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;

        public bool IsDeliverToFolder { get; set; }

        public string FolderDirectoryLocation { get; set; } = string.Empty;

        public bool UseDefaultCredentials { get; set; }

        public bool IsAuthenticatedSmtp { get; set; }

    }
}
