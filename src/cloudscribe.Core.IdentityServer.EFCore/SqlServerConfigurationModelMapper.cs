using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class SqlServerConfigurationModelMapper : IConfigurationModelMapper
    {
        public void Map(EntityTypeBuilder<Client> client)
        {
            client.ToTable(EfConstants.TableNames.Client).HasKey(x => x.Id);
            client.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
            client.HasIndex(x => x.SiteId);
            client.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
            client.Property(x => x.ClientName).HasMaxLength(200).IsRequired();
            client.Property(x => x.ClientUri).HasMaxLength(2000);

            client.HasMany(x => x.AllowedGrantTypes).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.RedirectUris).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.PostLogoutRedirectUris).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.AllowedScopes).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.ClientSecrets).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.Claims).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.IdentityProviderRestrictions).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            client.HasMany(x => x.AllowedCorsOrigins).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);

        }

        public void Map(EntityTypeBuilder<ClientGrantType> grantType)
        {
            grantType.ToTable(EfConstants.TableNames.ClientGrantType);
            grantType.Property(x => x.GrantType).HasMaxLength(250).IsRequired();
        }

        public void Map(EntityTypeBuilder<ClientRedirectUri> redirectUri)
        {
            redirectUri.ToTable(EfConstants.TableNames.ClientRedirectUri);
            redirectUri.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();
        }

        public void Map(EntityTypeBuilder<ClientPostLogoutRedirectUri> postLogoutRedirectUri)
        {
            postLogoutRedirectUri.ToTable(EfConstants.TableNames.ClientPostLogoutRedirectUri);
            postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();
        }

        public void Map(EntityTypeBuilder<ClientScope> scope)
        {
            scope.ToTable(EfConstants.TableNames.ClientScopes);
            scope.Property(x => x.Scope).HasMaxLength(200).IsRequired();

        }

        public void Map(EntityTypeBuilder<ClientSecret> secret)
        {
            secret.ToTable(EfConstants.TableNames.ClientSecret);
            secret.Property(x => x.Value).HasMaxLength(250).IsRequired();
            secret.Property(x => x.Type).HasMaxLength(250);
            secret.Property(x => x.Description).HasMaxLength(2000);
        }

        public void Map(EntityTypeBuilder<ClientClaim> claim)
        {
            claim.ToTable(EfConstants.TableNames.ClientClaim);
            claim.Property(x => x.Type).HasMaxLength(250).IsRequired();
            claim.Property(x => x.Value).HasMaxLength(250).IsRequired();
        }

        public void Map(EntityTypeBuilder<ClientIdPRestriction> idPRestriction)
        {
            idPRestriction.ToTable(EfConstants.TableNames.ClientIdPRestriction);
            idPRestriction.Property(x => x.Provider).HasMaxLength(200).IsRequired();
        }

        public void Map(EntityTypeBuilder<ClientCorsOrigin> corsOrigin)
        {
            corsOrigin.ToTable(EfConstants.TableNames.ClientCorsOrigin);
            corsOrigin.Property(x => x.Origin).HasMaxLength(150).IsRequired();
        }


        public void Map(EntityTypeBuilder<ScopeClaim> scopeClaim)
        {
            scopeClaim.ToTable(EfConstants.TableNames.ScopeClaim).HasKey(x => x.Id);
            scopeClaim.Property(x => x.Name).HasMaxLength(200).IsRequired();
            scopeClaim.Property(x => x.Description).HasMaxLength(1000);
        }

        public void Map(EntityTypeBuilder<ScopeSecret> scopeSecret)
        {
            scopeSecret.ToTable(EfConstants.TableNames.ScopeSecrets).HasKey(x => x.Id);
            scopeSecret.Property(x => x.Description).HasMaxLength(1000);
            scopeSecret.Property(x => x.Type).HasMaxLength(250);
            scopeSecret.Property(x => x.Value).HasMaxLength(250);
        }

        public void Map(EntityTypeBuilder<Scope> scope)
        {
            scope.ToTable(EfConstants.TableNames.Scope).HasKey(x => x.Id);
            scope.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
            scope.HasIndex(x => x.SiteId);
            scope.Property(x => x.Name).HasMaxLength(200).IsRequired();
            scope.Property(x => x.DisplayName).HasMaxLength(200);
            scope.Property(x => x.Description).HasMaxLength(1000);
            scope.Property(x => x.ClaimsRule).HasMaxLength(200);
            scope.HasMany(x => x.Claims).WithOne(x => x.Scope).IsRequired().OnDelete(DeleteBehavior.Cascade);
            scope.HasMany(x => x.ScopeSecrets).WithOne(x => x.Scope).IsRequired().OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
