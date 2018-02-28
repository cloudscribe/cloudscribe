using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.SendGrid
{
    public class ConfigSendGridOptionsProvider : ISendGridOptionsProvider
    {
        public ConfigSendGridOptionsProvider(IOptions<SendGridOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        private SendGridOptions _options;

        public Task<SendGridOptions> GetSendGridOptions()
        {
            return Task.FromResult(_options);
        }
    }
}
