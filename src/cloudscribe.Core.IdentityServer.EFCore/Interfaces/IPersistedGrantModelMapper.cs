using cloudscribe.Core.IdentityServer.EFCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.Core.IdentityServer.EFCore.Interfaces
{
    public interface IPersistedGrantModelMapper
    {
        void Map(EntityTypeBuilder<PersistedGrant> entity);
    }
}
