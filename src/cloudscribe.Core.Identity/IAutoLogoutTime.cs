using System.Threading;
using System;

namespace cloudscribe.Core.Identity
{
    public interface IAutoLogoutTime
    {
        string GetMaximumInactivityMinutes(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
