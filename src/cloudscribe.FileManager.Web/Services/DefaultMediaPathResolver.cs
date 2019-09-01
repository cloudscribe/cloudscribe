using cloudscribe.FileManager.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Services
{
    public class DefaultMediaPathResolver : IMediaPathResolver
    {
        public DefaultMediaPathResolver(
            IWebHostEnvironment environment)
        {
            hosting = environment;
        }

        private IWebHostEnvironment hosting;

        public Task<MediaRootPathInfo> Resolve(CancellationToken cancellationToken = default(CancellationToken))
        {
            var virtualPath = "";
            var fsPath = hosting.WebRootPath;
            var result = new MediaRootPathInfo(virtualPath, fsPath);
            return Task.FromResult(result);

        }
    }
}
