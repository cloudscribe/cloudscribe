using cloudscribe.Messaging.Email.ElasticEmail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteElasticEmailOptionsProvider : ConfigElasticEmailOptionsProvider
    {
        public SiteElasticEmailOptionsProvider(
            SiteManager siteManager,
            ILogger<SiteElasticEmailOptionsProvider> logger,
            IOptions<ElasticEmailOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteManager = siteManager;
            _log = logger;
        }

        private SiteManager _siteManager;
        private ILogger _log;

        public override async Task<ElasticEmailOptions> GetElasticEmailOptions(string lookupKey = null)
        {
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    var site = await _siteManager.Fetch(new Guid(lookupKey));
                    if (site != null)
                    {
                        if (site.EmailSenderName == "ElasticEmailSender"
                            && !string.IsNullOrWhiteSpace(site.EmailApiKey)
                            )
                        {
                            return new ElasticEmailOptions
                            {
                                ApiKey = site.EmailApiKey,
                                EndpointUrl = site.EmailApiEndpoint,
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

            return await base.GetElasticEmailOptions(lookupKey);
        }

    }
}
