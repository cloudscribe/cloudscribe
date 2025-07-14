using System;
using System.Net;

namespace cloudscribe.Core.Web.Components
{
    public interface IBlockedOrPermittedIpService
    {
        bool IsBlockedOrPermittedIp(IPAddress ipAddress, Guid siteId);
    }
}