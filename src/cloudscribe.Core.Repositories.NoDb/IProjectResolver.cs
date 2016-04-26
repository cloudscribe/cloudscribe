

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.NoDb
{
    public interface IProjectResolver
    {
        Task<string> ResolveProjectId(CancellationToken cancellationToken = default(CancellationToken));
    }
}
