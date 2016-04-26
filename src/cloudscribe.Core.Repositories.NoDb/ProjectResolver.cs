using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.NoDb
{
    // TODO: implement, for now just returning hard coded project named common
    // but ultimately would like to map project to site so storage can be segragated by siteid as the projectid
    // this will require keeping a central list to map projectid to site and site to proejctid
    // will need to be able to resolve that tenant resolution from this central list
    // also when sites are created or deleted or host/folder mapping changes the central list must be updated
    public class ProjectResolver : IProjectResolver
    {
        public ProjectResolver(
            IEnumerable<ProjectMapping> projectMappings)
        {
            this.projectMappings = projectMappings;
        }

        private IEnumerable<ProjectMapping> projectMappings;

        public Task<string> ResolveProjectId(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = "common";

            return Task.FromResult(result);
        }

        
    }

    
}
