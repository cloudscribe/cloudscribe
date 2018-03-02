namespace cloudscribe.Messaging.Email.Mailgun
{
    public class MailgunOptions
    {
        public string DomainName { get; set; }
        public string ApiKey { get; set; }
        public string DefaultEmailFromAddress { get; set; }
        public string DefaultEmailFromAlias { get; set; }
    }
}
