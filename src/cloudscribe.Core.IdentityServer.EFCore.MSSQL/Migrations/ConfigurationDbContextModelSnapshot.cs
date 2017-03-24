using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.IdentityServer.EFCore.MSSQL;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    partial class ConfigurationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200);

                    b.Property<bool>("Enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("SiteId", "Name")
                        .IsUnique();

                    b.ToTable("csids_ApiResources");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResourceClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApiResourceId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ApiResourceId");

                    b.ToTable("csids_ApiClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiScope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApiResourceId")
                        .IsRequired();

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200);

                    b.Property<bool>("Emphasize");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("Required");

                    b.Property<bool>("ShowInDiscoveryDocument");

                    b.Property<string>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("ApiResourceId");

                    b.HasIndex("SiteId", "Name")
                        .IsUnique();

                    b.ToTable("csids_ApiScopes");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiScopeClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApiScopeId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ApiScopeId");

                    b.ToTable("csids_ApiScopeClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiSecret", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApiResourceId")
                        .IsRequired();

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<DateTime?>("Expiration");

                    b.Property<string>("Type")
                        .HasMaxLength(250);

                    b.Property<string>("Value")
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.HasIndex("ApiResourceId");

                    b.ToTable("csids_ApiSecrets");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AbsoluteRefreshTokenLifetime");

                    b.Property<int>("AccessTokenLifetime");

                    b.Property<int>("AccessTokenType");

                    b.Property<bool>("AllowAccessTokensViaBrowser");

                    b.Property<bool>("AllowOfflineAccess");

                    b.Property<bool>("AllowPlainTextPkce");

                    b.Property<bool>("AllowRememberConsent");

                    b.Property<bool>("AlwaysSendClientClaims");

                    b.Property<int>("AuthorizationCodeLifetime");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("ClientName")
                        .HasMaxLength(200);

                    b.Property<string>("ClientUri")
                        .HasMaxLength(2000);

                    b.Property<bool>("EnableLocalLogin");

                    b.Property<bool>("Enabled");

                    b.Property<int>("IdentityTokenLifetime");

                    b.Property<bool>("IncludeJwtId");

                    b.Property<string>("LogoUri");

                    b.Property<bool>("LogoutSessionRequired");

                    b.Property<string>("LogoutUri");

                    b.Property<bool>("PrefixClientClaims");

                    b.Property<string>("ProtocolType")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("RefreshTokenExpiration");

                    b.Property<int>("RefreshTokenUsage");

                    b.Property<bool>("RequireClientSecret");

                    b.Property<bool>("RequireConsent");

                    b.Property<bool>("RequirePkce");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<int>("SlidingRefreshTokenLifetime");

                    b.Property<bool>("UpdateAccessTokenClaimsOnRefresh");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("SiteId", "ClientId")
                        .IsUnique();

                    b.ToTable("csids_Clients");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientCorsOrigin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientCorsOrigins");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientGrantType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("GrantType")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientGrantTypes");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientIdPRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientIdPRestrictions");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientPostLogoutRedirectUri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("PostLogoutRedirectUri")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientPostLogoutRedirectUris");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientRedirectUri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientRedirectUris");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientScope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("Scope")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientScopes");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientSecret", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId")
                        .IsRequired();

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<DateTime?>("Expiration");

                    b.Property<string>("Type")
                        .HasMaxLength(250);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientSecrets");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.IdentityClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("IdentityResourceId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("IdentityResourceId");

                    b.ToTable("csids_IdentityClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.IdentityResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200);

                    b.Property<bool>("Emphasize");

                    b.Property<bool>("Enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("Required");

                    b.Property<bool>("ShowInDiscoveryDocument");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("SiteId", "Name")
                        .IsUnique();

                    b.ToTable("csids_IdentityResources");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResourceClaim", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResource", "ApiResource")
                        .WithMany("UserClaims")
                        .HasForeignKey("ApiResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiScope", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResource", "ApiResource")
                        .WithMany("Scopes")
                        .HasForeignKey("ApiResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiScopeClaim", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiScope", "ApiScope")
                        .WithMany("UserClaims")
                        .HasForeignKey("ApiScopeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiSecret", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.ApiResource", "ApiResource")
                        .WithMany("Secrets")
                        .HasForeignKey("ApiResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientClaim", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("Claims")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientCorsOrigin", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("AllowedCorsOrigins")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientGrantType", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("AllowedGrantTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientIdPRestriction", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("IdentityProviderRestrictions")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientPostLogoutRedirectUri", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("PostLogoutRedirectUris")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientRedirectUri", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("RedirectUris")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientScope", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("AllowedScopes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ClientSecret", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", "Client")
                        .WithMany("ClientSecrets")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.IdentityClaim", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.IdentityResource", "IdentityResource")
                        .WithMany("UserClaims")
                        .HasForeignKey("IdentityResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
