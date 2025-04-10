﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteAccountCapabilitiesProvider
    {
        Task<bool> SupportsEmailNotification(ISiteContext site);
        
        Task<bool> SupportsSmsNotification(ISiteContext site);

        int GetPasswordExpiryWarningDays(ISiteContext site);
        int GetPasswordExpiryDays(ISiteContext site);
    }

    public class DefaultSiteAcountCapabilitiesProvider : ISiteAccountCapabilitiesProvider
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

        public int GetPasswordExpiryWarningDays(ISiteContext site)
        {
            return site.PasswordExpiryWarningDays;
        }

        public int GetPasswordExpiryDays(ISiteContext site)
        {
            return site.PasswordExpiresDays;
        }
    }
}
