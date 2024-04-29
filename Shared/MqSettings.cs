using System;

namespace Shared
{
    public class MqSettings
    {
        public const string Key = nameof(MqSettings);
        public string Url { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public MqSettings()
        {
            Url = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
