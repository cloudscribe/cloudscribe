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
            SiteManager siteManager,
            IEnumerable<IEmailSender> allConfiguredSenders,
            ILogger<SiteEmailSenderResolver> logger
            ):base(allConfiguredSenders)
        {
            _allConfiguredSenders = allConfiguredSenders;
            _siteManager = siteManager;
            _log = logger;
        }

        private SiteManager _siteManager;
        private IEnumerable<IEmailSender> _allConfiguredSenders;
        private ILogger _log;

        public override async Task<IEmailSender> GetEmailSender(string lookupKey = null)
        {
            // expected lookupKey in cloudscribe is siteId
            // site specific settings override config settings if configured
            if(!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    var site = await _siteManager.Fetch(new Guid(lookupKey));
                    if(site != null)
                    { 
                        foreach(var sender in _allConfiguredSenders)
                        {
                            if(sender.Name == site.EmailSenderName)
                            {
                                var configured = await sender.IsConfigured(site.Id.ToString());
                                if(configured) { return sender; }
                            }
                        }
                   

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

            // return sender from config if no site specific configured sender if found
            return await base.GetEmailSender(lookupKey);
            
        }

    }
}
