using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.Mailgun
{
    public interface IMailgunOptionsProvider
    {
        Task<MailgunOptions> GetMailgunOptions(string lookupKey = null);
    }
}
