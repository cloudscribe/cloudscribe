using cloudscribe.Core.IdentityServer.EFCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.Core.IdentityServer.EFCore.Interfaces
{
    public interface IConfigurationModelMapper
    {
        void Map(EntityTypeBuilder<Client> entity);

        void Map(EntityTypeBuilder<ClientGrantType> entity);

        void Map(EntityTypeBuilder<ClientRedirectUri> entity);

        void Map(EntityTypeBuilder<ClientPostLogoutRedirectUri> entity);

        void Map(EntityTypeBuilder<ClientScope> entity);

        void Map(EntityTypeBuilder<ClientSecret> entity);

        void Map(EntityTypeBuilder<ClientClaim> entity);

        void Map(EntityTypeBuilder<ClientIdPRestriction> entity);

        void Map(EntityTypeBuilder<ClientCorsOrigin> entity);


        void Map(EntityTypeBuilder<ScopeClaim> entity);
        void Map(EntityTypeBuilder<ScopeSecret> entity);
        void Map(EntityTypeBuilder<Scope> entity);
    }
}
