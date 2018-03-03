using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteAcountCapabilitiesProvider
    {
        Task<bool> SupportsEmailNotification(ISiteContext site);
        
        Task<bool> SupportsSmsNotification(ISiteContext site);
    }

    public class DefaultSiteAcountCapabilitiesProvider : ISiteAcountCapabilitiesProvider
    {
        public Task<bool> SupportsEmailNotification(ISiteContext site)
        {
            var result = !string.IsNullOrEmpty(site.SmtpServer);
            return Task.FromResult(result);
        }

        public Task<bool> SupportsSmsNotification(ISiteContext site)
        {
            var result = !string.IsNullOrEmpty(site.SmsFrom);
            return Task.FromResult(result);
        }

    }
}
