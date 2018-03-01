using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.Smtp
{
    public interface ISmtpOptionsProvider
    {
        Task<SmtpOptions> GetSmtpOptions(string lookupKey = null);
    }
}
