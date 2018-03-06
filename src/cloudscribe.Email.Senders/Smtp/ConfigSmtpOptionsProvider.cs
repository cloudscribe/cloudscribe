using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Email.Smtp
{
    public class ConfigSmtpOptionsProvider : ISmtpOptionsProvider
    {
        public ConfigSmtpOptionsProvider(IOptions<SmtpOptions> smtpOptionsAccessor)
        {
            _smtpSettings = smtpOptionsAccessor.Value;
        }

        private SmtpOptions _smtpSettings;

        public virtual Task<SmtpOptions> GetSmtpOptions(string lookupKey = null)
        {
            return Task.FromResult(_smtpSettings);
        }
    }
}
