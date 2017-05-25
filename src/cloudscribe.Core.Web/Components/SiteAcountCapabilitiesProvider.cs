using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteAcountCapabilitiesProvider : ISiteAcountCapabilitiesProvider
    {
        public SiteAcountCapabilitiesProvider(
            ISmtpOptionsProvider smtpOptionsProvider
            )
        {
            this.smtpOptionsProvider = smtpOptionsProvider;
        }

        private readonly ISmtpOptionsProvider smtpOptionsProvider;
        private SmtpOptions smtpOptions = null;

        public async Task<bool> SupportsEmailNotification(ISiteContext site)
        {
            if (smtpOptions == null) { smtpOptions = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false); }
            if(smtpOptions != null)
            {
                return !string.IsNullOrEmpty(smtpOptions.Server);
            }
            return false;
            
        }

        public Task<bool> SupportsSmsNotification(ISiteContext site)
        {
            var result = !string.IsNullOrEmpty(site.SmsFrom);
            return Task.FromResult(result);
        }

    }
}
