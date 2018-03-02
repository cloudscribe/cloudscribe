namespace cloudscribe.Messaging.Email.Mailgun
{
    public class MailgunOptions
    {
        public string EndpointUrl { get; set; }
        public string ApiKey { get; set; }
        public string DefaultEmailFromAddress { get; set; }
        public string DefaultEmailFromAlias { get; set; }
    }
}
