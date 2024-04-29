namespace MailService.Configurations
{
    public class AppConfigurations
    {
        public const string Key = "AppSettings";

        //Timeout for connection to queue in seconds.
        public int QueueConnectionTimeOut { get; set; }
    }
}
