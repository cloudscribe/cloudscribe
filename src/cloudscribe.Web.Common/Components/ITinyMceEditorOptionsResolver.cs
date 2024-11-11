using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Components
{
    public interface ITinyMceEditorOptionsResolver
    {
        Task<TinyMceEditorOptions> GetTinyMceEditorOptions();
    }

    public class DefaultTinyMceEditorOptionsResolver : ITinyMceEditorOptionsResolver
    {
        public DefaultTinyMceEditorOptionsResolver(IOptions<TinyMceEditorOptions> mceOptionsAccessor)
        {
            options = mceOptionsAccessor.Value;
        }

        private TinyMceEditorOptions options;

        public Task<TinyMceEditorOptions> GetTinyMceEditorOptions()
        {
            return Task.FromResult(options);
        }

    }
}