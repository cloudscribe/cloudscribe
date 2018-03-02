namespace cloudscribe.Messaging.Email.ElasticEmail
{
    public class ElasticEmailOptions
    {
        public string ApiKey { get; set; }
        public string EndpointUrl { get; set; } = "https://api.elasticemail.com/v2/email/send";
        public string DefaultEmailFromAddress { get; set; }
        public string DefaultEmailFromAlias { get; set; } 
    }


}
