using IdentityServer4.Models;
using System;

namespace cloudscribe.Core.IdentityServer.NoDb.Models
{
    public class GrantItem : PersistedGrant
    {
        public GrantItem() : base()
        {

        }

        public GrantItem(PersistedGrant grant) : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ClientId = grant.ClientId;
            this.CreationTime = grant.CreationTime;
            this.Data = grant.Data;
            this.Expiration = grant.Expiration;
            this.Key = grant.Key;
            this.SubjectId = grant.SubjectId;
            this.Type = grant.Type;

        }


        public string Id { get; set; }
    }
}
