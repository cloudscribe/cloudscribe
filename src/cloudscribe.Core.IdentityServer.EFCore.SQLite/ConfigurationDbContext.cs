using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.SQLite
{
    public class ConfigurationDbContext : ConfigurationDbContextBase, IConfigurationDbContext
    {
        public ConfigurationDbContext(
            DbContextOptions<ConfigurationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.Client)
                .HasKey(x => x.Id);
                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ClientName).HasMaxLength(200);
                entity.Property(x => x.ClientUri).HasMaxLength(2000);
                //new in 2.0
                entity.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
                entity.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
                entity.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);

                entity.HasIndex(x => new { x.SiteId, x.ClientId })
                .IsUnique();

                entity.HasMany(x => x.AllowedGrantTypes).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.RedirectUris).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.PostLogoutRedirectUris).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.AllowedScopes).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.ClientSecrets).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.Claims).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.IdentityProviderRestrictions).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.AllowedCorsOrigins).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<ClientGrantType>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientGrantType);
                entity.Property(x => x.GrantType).HasMaxLength(250).IsRequired();

            });

            modelBuilder.Entity<ClientRedirectUri>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientRedirectUri);
                entity.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();

            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientPostLogoutRedirectUri);
                entity.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();

            });

            modelBuilder.Entity<ClientProperty>(b =>
            {
                b.ToTable(EfConstants.TableNames.ClientProps);
                b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Key")
                    .IsRequired()
                    .HasMaxLength(250);

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ClientId");

            });

            modelBuilder.Entity<ClientScope>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientScopes);
                entity.Property(x => x.Scope).HasMaxLength(200).IsRequired();

            });

            modelBuilder.Entity<ClientSecret>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientSecret);
                entity.Property(x => x.Value).HasMaxLength(250).IsRequired();
                entity.Property(x => x.Type).HasMaxLength(250);
                entity.Property(x => x.Description).HasMaxLength(2000);

            });

            modelBuilder.Entity<ClientClaim>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientClaim);
                entity.Property(x => x.Type).HasMaxLength(250).IsRequired();
                entity.Property(x => x.Value).HasMaxLength(250).IsRequired();

            });

            modelBuilder.Entity<ClientIdPRestriction>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientIdPRestriction);
                entity.Property(x => x.Provider).HasMaxLength(200).IsRequired();

            });

            modelBuilder.Entity<ClientCorsOrigin>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ClientCorsOrigin);
                entity.Property(x => x.Origin).HasMaxLength(150).IsRequired();

            });


            modelBuilder.Entity<IdentityResource>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.IdentityResource)
                .HasKey(x => x.Id);

                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DisplayName).HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);

                entity.HasIndex(x => new { x.SiteId, x.Name })
                .IsUnique();

                entity.HasMany(x => x.UserClaims).WithOne(x => x.IdentityResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityClaim>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.IdentityClaim)
                .HasKey(x => x.Id);

                entity.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });


            modelBuilder.Entity<ApiResource>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ApiResource)
                .HasKey(x => x.Id);

                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DisplayName).HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);

                entity.HasIndex(x => new { x.SiteId, x.Name })
                .IsUnique();

                entity.HasMany(x => x.Secrets).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.Scopes).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.UserClaims).WithOne(x => x.ApiResource).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiSecret>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ApiSecret)
                .HasKey(x => x.Id);

                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.Value).HasMaxLength(2000);
                entity.Property(x => x.Type).HasMaxLength(250);
            });

            modelBuilder.Entity<ApiResourceClaim>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ApiClaim)
                .HasKey(x => x.Id);

                entity.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ApiScope>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ApiScope)
                .HasKey(x => x.Id);

                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DisplayName).HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);

                entity.HasIndex(x => new { x.SiteId, x.Name })
                .IsUnique();

                entity.HasMany(x => x.UserClaims).WithOne(x => x.ApiScope).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiScopeClaim>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ApiScopeClaim)
                .HasKey(x => x.Id);

                entity.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });


        }
    }
}
