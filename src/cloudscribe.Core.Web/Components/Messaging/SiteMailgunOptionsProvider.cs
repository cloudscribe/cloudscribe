using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email.Mailgun;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteMailgunOptionsProvider : ConfigMailgunOptionsProvider
    {
        public SiteMailgunOptionsProvider(
            ISiteQueries siteQueries,
            ILogger<SiteMailgunOptionsProvider> logger,
            IOptions<MailgunOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteQueries = siteQueries;
            _log = logger;
        }

        private ISiteQueries _siteQueries;
        private ILogger _log;

        public override async Task<MailgunOptions> GetMailgunOptions(string lookupKey = null)
        {
            //TODO:

            return await base.GetMailgunOptions(lookupKey);
        }
    }
}
