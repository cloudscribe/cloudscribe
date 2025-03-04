using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Components
{
    public interface ISummernoteOptionsResolver
    {
        Task<SummernoteOptions> GetSummernoteOptions();
    }

    public class DefaultSummernoteOptionsResolver : ISummernoteOptionsResolver
    {
        public DefaultSummernoteOptionsResolver(IOptions<SummernoteOptions> summernoteOptionsAccessor)
        {
            options = summernoteOptionsAccessor.Value;
        }

        private SummernoteOptions options;

        public Task<SummernoteOptions> GetSummernoteOptions()
        {
            return Task.FromResult(options);
        }

    }
}
