using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class SqlServerPersistedGrantModelMapper : IPersistedGrantModelMapper
    {
        public void Map(EntityTypeBuilder<PersistedGrant> grant)
        {
            grant.ToTable(EfConstants.TableNames.PersistedGrant);
            grant.HasKey(x => new { x.Key, x.Type });
            grant.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
            grant.HasIndex(x => x.SiteId);
            grant.Property(x => x.SubjectId);
            grant.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
            grant.Property(x => x.CreationTime).IsRequired();
            grant.Property(x => x.Expiration).IsRequired();
            grant.Property(x => x.Data).IsRequired();
        }
    }
}
