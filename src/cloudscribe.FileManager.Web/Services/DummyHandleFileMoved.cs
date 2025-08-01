using cloudscribe.FileManager.Web.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Services
{
    public class DummyHandleFileMoved : IHandleFileMoved
    {
        private readonly ILogger<FileManagerService> logger;

        public DummyHandleFileMoved(ILogger<FileManagerService> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(string siteId, string oldUrl, string newUrl)
        {
            logger.LogDebug($"File Moved Handler (Dummy): you may want to replace all instances of {oldUrl} by {newUrl} for site {siteId}");
            await Task.CompletedTask;
        }
    }
}
