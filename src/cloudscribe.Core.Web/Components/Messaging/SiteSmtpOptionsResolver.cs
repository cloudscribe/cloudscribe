using cloudscribe.Core.Models;
using cloudscribe.Email.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSmtpOptionsResolver : ConfigSmtpOptionsProvider
    {
        public SiteSmtpOptionsResolver(
            ISiteContextResolver siteResolver,
            ILogger<SiteSmtpOptionsResolver> logger,
            IOptions<SmtpOptions> smtpOptionsAccessor
            ):base(smtpOptionsAccessor)
        {
            _siteResolver = siteResolver;
            _log = logger;
        }

        private ISiteContextResolver _siteResolver;
        private ILogger _log;
        
        public override async Task<SmtpOptions> GetSmtpOptions(string lookupKey = null)
        {
            ISiteContext currentSite = null;
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    currentSite = await _siteResolver.GetById(new Guid(lookupKey));
                    if (currentSite != null)
                    {
                        if (string.IsNullOrEmpty(currentSite.SmtpServer)) { return await base.GetSmtpOptions(lookupKey); }

                        SmtpOptions smtpOptions = new SmtpOptions
                        {
                            Password = currentSite.SmtpPassword,
                            Port = currentSite.SmtpPort,
                            PlainTextBodyDefaultEncoding = currentSite.SmtpPreferredEncoding,
                            RequiresAuthentication = currentSite.SmtpRequiresAuth,
                            Server = currentSite.SmtpServer,
                            User = currentSite.SmtpUser,
                            UseSsl = currentSite.SmtpUseSsl,
                            DefaultEmailFromAddress = currentSite.DefaultEmailFromAddress,
                            DefaultEmailFromAlias = currentSite.DefaultEmailFromAlias
                        };

                        return smtpOptions;
                    }
                    else
                    {
                        _log.LogError($"failed to lookup site to get email settings, no site found using lookupKey {lookupKey}");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"failed to lookup site to get email settings, lookupKey was not a valid guid string for siteid. {ex.Message} - {ex.StackTrace}");
                }
            }

            return await base.GetSmtpOptions(lookupKey);

        }
    }
}
