namespace Shared.Contracts
{
    public interface IEmailMessage
    {
        string Body { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string To { get; set; }
    }
}