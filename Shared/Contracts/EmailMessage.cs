using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public class EmailMessage : IEmailMessage
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailMessage()
        {
            From = "donotreply@manu.com";
            To = string.Empty;
            Subject = string.Empty;
            Body = string.Empty;
        }
    }
}
