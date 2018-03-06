using System.Threading.Tasks;

namespace cloudscribe.Email
{
    public interface IEmailSenderResolver
    {
        Task<IEmailSender> GetEmailSender(string lookupKey = null);
    }
}
