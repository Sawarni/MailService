using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace SmtpService.Configurations
{
    public class MailRateConfiguration
    {
        public const string Key = nameof(MailRateConfiguration);

        /// <summary>
        /// Number of emails aloowed to be sent in particular duration. The duration can be specified by <seealso cref="InDuration"/>.
        /// </summary>
        public int AllowedMailQuota { get; set; }

        /// <summary>
        /// Duration for which only a limited mail quota can be sent. Can be configured by <seealso cref="AllowedMailQuota"/>
        /// </summary>
        public int InDuration { get; set; }

    }
}
