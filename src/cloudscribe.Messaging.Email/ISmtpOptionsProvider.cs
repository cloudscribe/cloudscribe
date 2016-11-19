using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{
    public interface ISmtpOptionsProvider
    {
        Task<SmtpOptions> GetSmtpOptions();
    }
}
