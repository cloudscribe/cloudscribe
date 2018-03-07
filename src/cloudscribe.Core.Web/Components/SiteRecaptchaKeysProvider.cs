using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
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
            _currentSite = currentSite;
            _configKeys = keysAccessor.Value;
        }

        private SiteContext _currentSite;
        private RecaptchaKeys _configKeys;

        public Task<RecaptchaKeys> GetKeys()
        {
            if(!string.IsNullOrEmpty(_currentSite.RecaptchaPublicKey))
            {
                var siteKeys = new RecaptchaKeys
                {
                    PrivateKey = _currentSite.RecaptchaPrivateKey,
                    PublicKey = _currentSite.RecaptchaPublicKey,
                    Invisible = _currentSite.UseInvisibleRecaptcha
                };
                return Task.FromResult(siteKeys);
            }

            return Task.FromResult(_configKeys);
        }
    }
}
