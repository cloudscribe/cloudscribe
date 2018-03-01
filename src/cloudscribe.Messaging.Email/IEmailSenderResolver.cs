using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{
    public interface IEmailSenderResolver
    {
        Task<IEmailSender> GetEmailSender(string lookupKey = null);
    }
}
