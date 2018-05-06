namespace cloudscribe.Web.Common.Models
{
    public class RecaptchaKeys
    {
        public string PrivateKey { get; set; } = string.Empty;

        public string PublicKey { get; set; } = string.Empty;

        public bool Invisible { get; set; } = false;
    }
}
