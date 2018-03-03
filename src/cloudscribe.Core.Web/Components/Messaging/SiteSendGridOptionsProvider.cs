using cloudscribe.Messaging.Email.SendGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSendGridOptionsProvider : ConfigSendGridOptionsProvider
    {
        public SiteSendGridOptionsProvider(
            SiteManager siteManager,
            ILogger<SiteMailgunOptionsProvider> logger,
            IOptions<SendGridOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteManager = siteManager;
            _log = logger;
        }

        private SiteManager _siteManager;
        private ILogger _log;

        public override async Task<SendGridOptions> GetSendGridOptions(string lookupKey = null)
        {
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    var site = await _siteManager.Fetch(new Guid(lookupKey));
                    if (site != null)
                    {
                        if (site.EmailSenderName == "SendGridEmailSender"
                            && !string.IsNullOrWhiteSpace(site.EmailApiKey)
                            )
                        {
                            return new SendGridOptions
                            {
                                ApiKey = site.EmailApiKey,
                                DefaultEmailFromAddress = site.DefaultEmailFromAddress,
                                DefaultEmailFromAlias = site.DefaultEmailFromAlias
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"failed to lookup site to get email settings, lookupKey was not a valid guid string. {ex.Message} - {ex.StackTrace}");
                }
            }

            return await base.GetSendGridOptions(lookupKey);
        }

    }
}
