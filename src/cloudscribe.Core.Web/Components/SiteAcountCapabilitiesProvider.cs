using cloudscribe.Core.Models;
using cloudscribe.Email;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteAcountCapabilitiesProvider : ISiteAcountCapabilitiesProvider
    {
        public SiteAcountCapabilitiesProvider(
            IEmailSenderResolver emailSenderResolver
            )
        {
            _emailSenderResolver = emailSenderResolver;
        }
        
        private IEmailSenderResolver _emailSenderResolver;

        public async Task<bool> SupportsEmailNotification(ISiteContext site)
        {
            var sender = await _emailSenderResolver.GetEmailSender(site.Id.ToString());
            if(sender != null)
            {
                var configured = await sender.IsConfigured(site.Id.ToString());
                return configured;
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
