using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Models
{
    public interface IMediaPathResolver
    {
        Task<MediaRootPathInfo> Resolve(CancellationToken cancellationToken = default(CancellationToken));
    }
}
