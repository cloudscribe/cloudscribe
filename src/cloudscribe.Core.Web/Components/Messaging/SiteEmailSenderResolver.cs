using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailSenderResolver : ConfigEmailSenderResolver
    {
        public SiteEmailSenderResolver(
            ISiteQueries siteQueries,
            IEnumerable<IEmailSender> allConfiguredSenders,
            ILogger<SiteEmailSenderResolver> logger
            ):base(allConfiguredSenders)
        {
            _siteQueries = siteQueries;
            //_configSenders = allConfiguredSenders;
            _log = logger;
        }

        private ISiteQueries _siteQueries;
        //private IEnumerable<IEmailSender> _configSenders;
        private ILogger _log;

        public override async Task<IEmailSender> GetEmailSender(string lookupKey = null)
        {
            // expected lookupKey in cloudscribe is siteId
            if(!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    var site = await _siteQueries.Fetch(new Guid(lookupKey));
                    if(site != null)
                    {
                        //TODO: need new property on sitesettings for the name of the email sender to use

                   

                    }
                    else
                    {
                        _log.LogError($"failed to lookup site to get email settings, no site found using lookupKey {lookupKey}");
                    }
                }
                catch(Exception ex)
                {
                    _log.LogError($"failed to lookup site to get email settings, lookupKey was not a valid guid string. {ex.Message} - {ex.StackTrace}");
                }
                
            }

            return await base.GetEmailSender(lookupKey);
            
        }

    }
}
