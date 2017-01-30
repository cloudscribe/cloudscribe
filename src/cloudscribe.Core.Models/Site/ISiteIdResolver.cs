using System;

namespace cloudscribe.Core.Models
{
    public interface ISiteIdResolver
    {
        Guid GetCurrentSiteId();
    }
}
