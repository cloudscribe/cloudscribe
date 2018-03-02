using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.ElasticEmail
{
    public class ConfigElasticEmailOptionsProvider : IElasticEmailOptionsProvider
    {
        public ConfigElasticEmailOptionsProvider(IOptions<ElasticEmailOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        private ElasticEmailOptions _options;

        public virtual Task<ElasticEmailOptions> GetElasticEmailOptions(string lookupKey = null)
        {
            return Task.FromResult(_options);
        }

    }
}
