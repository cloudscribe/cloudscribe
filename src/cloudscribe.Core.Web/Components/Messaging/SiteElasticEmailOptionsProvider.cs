using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email.ElasticEmail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteElasticEmailOptionsProvider : ConfigElasticEmailOptionsProvider
    {
        public SiteElasticEmailOptionsProvider(
            ISiteQueries siteQueries,
            ILogger<SiteElasticEmailOptionsProvider> logger,
            IOptions<ElasticEmailOptions> optionsAccessor
            ):base(optionsAccessor)
        {
            _siteQueries = siteQueries;
            _log = logger;
        }

        private ISiteQueries _siteQueries;
        private ILogger _log;

        public override async Task<ElasticEmailOptions> GetElasticEmailOptions(string lookupKey = null)
        {
            //TODO:

            return await base.GetElasticEmailOptions(lookupKey);
        }

    }
}
