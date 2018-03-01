using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSmtpOptionsResolver : ISmtpOptionsProvider
    {
        //TODO: when email is sent as a task on a background thread
        // SiteContext cannot be resolved because there is no httprequest
        // need to pass in siteid as lookupkey

        public SiteSmtpOptionsResolver(
            ISiteQueries siteQueries,
            ILogger<SiteSmtpOptionsResolver> logger,
            IOptions<SmtpOptions> smtpOptionsAccessor
            )
        {
            _siteQueries = siteQueries;
            _globalSmtp = smtpOptionsAccessor.Value;
            _log = logger;
        }

        private SmtpOptions _globalSmtp;
        private ISiteQueries _siteQueries;
        private ILogger _log;

        public async Task<SmtpOptions> GetSmtpOptions(string lookupKey = null)
        {
            ISiteSettings currentSite = null;
            if (!string.IsNullOrWhiteSpace(lookupKey) && lookupKey.Length == 36)
            {
                try
                {
                    currentSite = await _siteQueries.Fetch(new Guid(lookupKey));
                    if (currentSite != null)
                    {
                        //TODO: need new property on sitesettings for the name of the email sender to use
                        if (string.IsNullOrEmpty(currentSite.SmtpServer)) { return _globalSmtp; }

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
                    _log.LogError($"failed to lookup site to get email settings, lookupKey was not a valid guid string. {ex.Message} - {ex.StackTrace}");
                }

            }

            return _globalSmtp;

        }
    }
}
