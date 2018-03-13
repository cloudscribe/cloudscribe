using cloudscribe.Core.Models;
using cloudscribe.Email.Mailgun;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteMailgunOptionsProvider : ConfigMailgunOptionsProvider
    {
        public SiteMailgunOptionsProvider(
            ISiteContextResolver siteResolver,
            ILogger<SiteMailgunOptionsProvider> logger,
            IOptions<MailgunOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteResolver = siteResolver;
            _log = logger;
        }

        private ISiteContextResolver _siteResolver;
        private ILogger _log;

        public override async Task<MailgunOptions> GetMailgunOptions(string lookupKey = null)
        {
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    var site = await _siteResolver.GetById(new Guid(lookupKey));
                    if (site != null)
                    {
                        if(site.EmailSenderName == "MailgunEmailSender" 
                            && !string.IsNullOrWhiteSpace(site.EmailApiKey) 
                            && !string.IsNullOrWhiteSpace(site.EmailApiEndpoint))
                        {
                            return new MailgunOptions
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

            return await base.GetMailgunOptions(lookupKey);
        }
    }
}
