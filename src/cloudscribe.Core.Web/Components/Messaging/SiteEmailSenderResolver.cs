using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailSenderResolver : IEmailSenderResolver
    {
        public SiteEmailSenderResolver(
            ISiteQueries siteQueries,
            IEnumerable<IEmailSender> allConfiguredSenders,
            ILogger<SiteEmailSenderResolver> logger
            )
        {
            _siteQueries = siteQueries;
            _configSenders = allConfiguredSenders;
            _log = logger;
        }

        private ISiteQueries _siteQueries;
        private IEnumerable<IEmailSender> _configSenders;
        private ILogger _log;

        public async Task<IEmailSender> GetEmailSender(string lookupKey = null)
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

            //next try lookup by name
            if (!string.IsNullOrEmpty(lookupKey))
            {
                foreach (var sender in _configSenders)
                {
                    if (sender.Name == lookupKey)
                    {
                        return sender;
                    }
                }
            }

            // return first configured one
            foreach (var sender in _configSenders)
            {
                var configured = await sender.IsConfigured();
                if (configured) { return sender; }
            }

            // last ditch return the first one in the list configured or not
            foreach (var sender in _configSenders)
            {
                return sender;
            }


            return null;

        }

    }
}
