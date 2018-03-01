using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSmtpOptionsResolver : ISmtpOptionsProvider
    {
        //TODO: when email is sent as a task on a background thread
        // SiteContext cannot be resolved because there is no httprequest
        // need to pass in siteid as lookupkey

        public SiteSmtpOptionsResolver(
            SiteContext currentSite,
            IOptions<SmtpOptions> smtpOptionsAccessor
            )
        {
            this.currentSite = currentSite;
            globalSmtp = smtpOptionsAccessor.Value;
        }

        private SiteContext currentSite;
        private SmtpOptions globalSmtp;

        public Task<SmtpOptions> GetSmtpOptions(string lookupKey = null)
        {
            if (string.IsNullOrEmpty(currentSite.SmtpServer)) { return Task.FromResult(globalSmtp); }

            SmtpOptions smtpOptions = new SmtpOptions();
            smtpOptions.Password = currentSite.SmtpPassword;
            smtpOptions.Port = currentSite.SmtpPort;
            smtpOptions.PreferredEncoding = currentSite.SmtpPreferredEncoding;
            smtpOptions.RequiresAuthentication = currentSite.SmtpRequiresAuth;
            smtpOptions.Server = currentSite.SmtpServer;
            smtpOptions.User = currentSite.SmtpUser;
            smtpOptions.UseSsl = currentSite.SmtpUseSsl;
            smtpOptions.DefaultEmailFromAddress = currentSite.DefaultEmailFromAddress;
            smtpOptions.DefaultEmailFromAlias = currentSite.DefaultEmailFromAlias;

            return Task.FromResult(smtpOptions);
        }
    }
}
