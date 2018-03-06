using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Email.Mailgun
{
    public class ConfigMailgunOptionsProvider : IMailgunOptionsProvider
    {
        public ConfigMailgunOptionsProvider(IOptions<MailgunOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        private MailgunOptions _options;

        public virtual Task<MailgunOptions> GetMailgunOptions(string lookupKey = null)
        {
            return Task.FromResult(_options);
        }


    }
}
