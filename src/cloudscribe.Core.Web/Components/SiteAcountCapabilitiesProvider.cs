using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using cloudscribe.Messaging.Email.Smtp;
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
            //TODO: account for other email providers
            if (smtpOptions == null) { smtpOptions = await smtpOptionsProvider.GetSmtpOptions(site.Id.ToString()).ConfigureAwait(false); }
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
