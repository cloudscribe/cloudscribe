using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{
    public class ConfigSmtpOptionsProvider : ISmtpOptionsProvider
    {
        public ConfigSmtpOptionsProvider(IOptions<SmtpOptions> smtpOptionsAccessor)
        {
            smtpSettings = smtpOptionsAccessor.Value;
        }

        private SmtpOptions smtpSettings;

        public Task<SmtpOptions> GetSmtpOptions()
        {
            return Task.FromResult(smtpSettings);
        }
    }
}
