using cloudscribe.Core.Models;
using cloudscribe.Email.Smtp;
using cloudscribe.Email.SmtpOAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSmtpOAuthOptionsResolver : ConfigSmtpOAuthOptionsProvider
    {
        public SiteSmtpOAuthOptionsResolver(
            ISiteContextResolver siteResolver,
            ILogger<SiteSmtpOAuthOptionsResolver> logger,
            IOptions<SmtpOAuthOptions> smtpOAuthOptionsAccessor
            ):base(smtpOAuthOptionsAccessor)
        {
            _siteResolver = siteResolver;
            _log = logger;
        }

        private ISiteContextResolver _siteResolver;
        private ILogger _log;

        public override async Task<SmtpOAuthOptions> GetSmtpOAuthOptions(string lookupKey = null)
        {
            ISiteContext currentSite = null;
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    currentSite = await _siteResolver.GetById(new Guid(lookupKey));
                    if (currentSite != null)
                    {
                        if (string.IsNullOrEmpty(currentSite.SmtpServer)) { return await base.GetSmtpOAuthOptions(lookupKey); }

                        SmtpOAuthOptions smtpOAuthOptions = new SmtpOAuthOptions
                        {
                            Port = currentSite.SmtpPort,
                            PlainTextBodyDefaultEncoding = currentSite.SmtpPreferredEncoding,
                            Server = currentSite.SmtpServer,
                            User = currentSite.SmtpUser,
                            UseSsl = currentSite.SmtpUseSsl,
                            AuthorizeEndpoint = currentSite.SmtpOauthAuthorizeEndpoint,
                            TokenEndpoint = currentSite.SmtpOauthTokenEndpoint,
                            ClientId = currentSite.SmtpOauthClientId,
                            ClientSecret = currentSite.SmtpOauthClientSecret,
                            ScopesCsv = currentSite.SmtpOauthScopesCsv,
                            DefaultEmailFromAddress = currentSite.DefaultEmailFromAddress,
                            DefaultEmailFromAlias = currentSite.DefaultEmailFromAlias
                        };

                        return smtpOAuthOptions;
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

            return await base.GetSmtpOAuthOptions(lookupKey);

        }
    }
}
