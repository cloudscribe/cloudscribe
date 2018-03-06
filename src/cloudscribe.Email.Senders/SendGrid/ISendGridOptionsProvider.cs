using System.Threading.Tasks;

namespace cloudscribe.Email.SendGrid
{
    public interface ISendGridOptionsProvider
    {
        Task<SendGridOptions> GetSendGridOptions(string lookupKey = null);
    }
}
