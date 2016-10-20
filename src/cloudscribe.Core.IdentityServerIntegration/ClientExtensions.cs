using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public static class ClientExtensions
    {
        public static bool HasClaim(this Client client, Claim claim)
        {
            foreach(var c in client.Claims)
            {
                if (c.Type == claim.Type && c.Value == claim.Value) return true;
            }

            return false;
        }
    }
}
