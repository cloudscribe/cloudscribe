using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Identity
{
    public class OidcTokenManagementOptions
    {
        public string Scheme { get; set; }
        public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(1);
        public bool RevokeRefreshTokenOnSignout { get; set; } = true;

    }
}
