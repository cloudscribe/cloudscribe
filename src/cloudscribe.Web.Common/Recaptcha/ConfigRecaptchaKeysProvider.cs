using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Components
{
    public class ConfigRecaptchaKeysProvider : IRecaptchaKeysProvider
    {
        public ConfigRecaptchaKeysProvider(IOptions<RecaptchaKeys> keysAccessor)
        {
            keys = keysAccessor.Value;
        }

        private RecaptchaKeys keys;

        public Task<RecaptchaKeys> GetKeys()
        {
            return Task.FromResult(keys);
        }
    }
}
