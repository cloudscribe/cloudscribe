namespace cloudscribe.Email.SendGrid
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string DefaultEmailFromAddress { get; set; }
        public string DefaultEmailFromAlias { get; set; }
    }
}
