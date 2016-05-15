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
            var result = "default";

            var context = contextAccesor.HttpContext;
            if (context == null) return Task.FromResult(result);
            var request = context.Request;
            if (request == null) return Task.FromResult(result);

            var folder = request.Path.StartingSegment();
            var host = request.Host.Value;
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

            
            return Task.FromResult(result);
        }
    }
}
