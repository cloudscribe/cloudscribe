using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Email.SendGrid
{
    public class ConfigSendGridOptionsProvider : ISendGridOptionsProvider
    {
        public ConfigSendGridOptionsProvider(IOptions<SendGridOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        private SendGridOptions _options;

        public virtual Task<SendGridOptions> GetSendGridOptions(string lookupKey = null)
        {
            return Task.FromResult(_options);
        }
    }
}
