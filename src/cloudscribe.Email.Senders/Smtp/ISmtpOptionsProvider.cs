using System.Threading.Tasks;

namespace cloudscribe.Email.Smtp
{
    public interface ISmtpOptionsProvider
    {
        Task<SmtpOptions> GetSmtpOptions(string lookupKey = null);
    }
}
