using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.SendGrid
{
    public interface ISendGridOptionsProvider
    {
        Task<SendGridOptions> GetSendGridOptions(string lookupKey = null);
    }
}
