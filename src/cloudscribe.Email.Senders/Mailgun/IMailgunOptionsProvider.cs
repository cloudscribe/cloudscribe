using System.Threading.Tasks;

namespace cloudscribe.Email.Mailgun
{
    public interface IMailgunOptionsProvider
    {
        Task<MailgunOptions> GetMailgunOptions(string lookupKey = null);
    }
}
