using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{
    public class ConfigSmtpOptionsProvider : ISmtpOptionsProvider
    {
        public ConfigSmtpOptionsProvider(IOptions<SmtpOptions> smtpOptionsAccessor)
        {
            _smtpSettings = smtpOptionsAccessor.Value;
        }

        private SmtpOptions _smtpSettings;

        public Task<SmtpOptions> GetSmtpOptions()
        {
            return Task.FromResult(_smtpSettings);
        }
    }
}
