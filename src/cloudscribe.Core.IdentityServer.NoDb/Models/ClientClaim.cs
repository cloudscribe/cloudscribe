using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb.Models
{
    /// <summary>
    /// this class is needed because there is no default constructor on System.Security.Claim
    /// so can't deserialize it
    /// </summary>
    public class ClientClaim
    {
        public string Id { get; set; }

        public string ClientId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
