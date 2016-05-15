using cloudscribe.Core.Models;
using Microsoft.AspNet.Http.Internal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class DefaultProjectResolver : IProjectResolver
    {
        public DefaultProjectResolver(
            HttpContextAccessor contextAccesor,
            IEnumerable<IProjectRequestMap> projectMaps
            )
        {
            this.contextAccesor = contextAccesor;
            this.projectMaps = projectMaps;
        }

        private HttpContextAccessor contextAccesor;
        private IEnumerable<IProjectRequestMap> projectMaps;

        public Task<string> ResolveProjectId(CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = contextAccesor.HttpContext.Request.Path.StartingSegment();
            var host = contextAccesor.HttpContext.Request.Host.Value;
            foreach(var map in projectMaps)
            {
                if(!string.IsNullOrEmpty(map.FirstFolderSegment))
                {
                    if(!string.IsNullOrEmpty(folder))
                    {
                        if (folder == map.FirstFolderSegment) return Task.FromResult(map.ProjectId);
                    }
                }

                if (!string.IsNullOrEmpty(map.HostName))
                {
                    if (!string.IsNullOrEmpty(host))
                    {
                        if (host == map.HostName) return Task.FromResult(map.ProjectId);
                    }
                }
            }

            var result = "default";
            return Task.FromResult(result);
        }
    }
}
