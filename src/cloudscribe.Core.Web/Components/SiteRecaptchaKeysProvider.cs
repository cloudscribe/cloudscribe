using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteRecaptchaKeysProvider : IRecaptchaKeysProvider
    {
        public SiteRecaptchaKeysProvider(
            SiteContext currentSite,
            IOptions<RecaptchaKeys> keysAccessor
            )
        {
            this.currentSite = currentSite;
            configKeys = keysAccessor.Value;
        }

        private SiteContext currentSite;
        private RecaptchaKeys configKeys;

        public Task<RecaptchaKeys> GetKeys()
        {
            if(!string.IsNullOrEmpty(currentSite.RecaptchaPublicKey))
            {
                var siteKeys = new RecaptchaKeys();
                siteKeys.PrivateKey = currentSite.RecaptchaPrivateKey;
                siteKeys.PublicKey = currentSite.RecaptchaPublicKey;
                return Task.FromResult(siteKeys);
            }

            return Task.FromResult(configKeys);
        }
    }
}
