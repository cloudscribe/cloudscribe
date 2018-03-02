using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email.SendGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSendGridOptionsProvider : ConfigSendGridOptionsProvider
    {
        public SiteSendGridOptionsProvider(
            ISiteQueries siteQueries,
            ILogger<SiteMailgunOptionsProvider> logger,
            IOptions<SendGridOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteQueries = siteQueries;
            _log = logger;
        }

        private ISiteQueries _siteQueries;
        private ILogger _log;

        public override async Task<SendGridOptions> GetSendGridOptions(string lookupKey = null)
        {
            //TODO:

            return await base.GetSendGridOptions(lookupKey);
        }

    }
}
