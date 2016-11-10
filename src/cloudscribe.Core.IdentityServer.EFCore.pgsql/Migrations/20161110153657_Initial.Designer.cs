using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.IdentityServer.EFCore.pgsql;

namespace cloudscribe.Core.IdentityServer.EFCore.pgsql.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20161110153657_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AbsoluteRefreshTokenLifetime");

                    b.Property<int>("AccessTokenLifetime");

                    b.Property<int>("AccessTokenType");

                    b.Property<bool>("AllowAccessToAllScopes");

                    b.Property<bool>("AllowAccessTokensViaBrowser");

                    b.Property<bool>("AllowPromptNone");

                    b.Property<bool>("AllowRememberConsent");

                    b.Property<bool>("AlwaysSendClientClaims");

                    b.Property<int>("AuthorizationCodeLifetime");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("ClientUri")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<bool>("EnableLocalLogin");

                    b.Property<bool>("Enabled");

                    b.Property<int>("IdentityTokenLifetime");

                    b.Property<bool>("IncludeJwtId");

                    b.Property<string>("LogoUri");

                    b.Property<bool>("LogoutSessionRequired");

                    b.Property<string>("LogoutUri");

                    b.Property<bool>("PrefixClientClaims");

                    b.Property<bool>("PublicClient");

                    b.Property<int>("RefreshTokenExpiration");

                    b.Property<int>("RefreshTokenUsage");

                    b.Property<bool>("RequireClientSecret");

                    b.Property<bool>("RequireConsent");

                    b.Property<bool>("RequirePkce");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<int>("SlidingRefreshTokenLifetime");

                    b.Property<bool>("UpdateAccessTokenClaimsOnRefresh");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

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
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 250);

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
                        .HasAnnotation("MaxLength", 150);

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
                        .HasAnnotation("MaxLength", 250);

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
                        .HasAnnotation("MaxLength", 200);

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
                        .HasAnnotation("MaxLength", 2000);

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
                        .HasAnnotation("MaxLength", 2000);

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
                        .HasAnnotation("MaxLength", 200);

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
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<DateTime?>("Expiration");

                    b.Property<string>("Type")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 250);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("csids_ClientSecrets");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.Scope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowUnrestrictedIntrospection");

                    b.Property<string>("ClaimsRule")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1000);

                    b.Property<string>("DisplayName")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("Emphasize");

                    b.Property<bool>("Enabled");

                    b.Property<bool>("IncludeAllClaimsForUser");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("Required");

                    b.Property<bool>("ShowInDiscoveryDocument");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("csids_Scopes");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ScopeClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AlwaysIncludeInIdToken");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int?>("ScopeId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ScopeId");

                    b.ToTable("csids_ScopeClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ScopeSecret", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1000);

                    b.Property<DateTime?>("Expiration");

                    b.Property<int?>("ScopeId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("Value")
                        .HasAnnotation("MaxLength", 250);

                    b.HasKey("Id");

                    b.HasIndex("ScopeId");

                    b.ToTable("csids_ScopeSecrets");
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

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ScopeClaim", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Scope", "Scope")
                        .WithMany("Claims")
                        .HasForeignKey("ScopeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.ScopeSecret", b =>
                {
                    b.HasOne("cloudscribe.Core.IdentityServer.EFCore.Entities.Scope", "Scope")
                        .WithMany("ScopeSecrets")
                        .HasForeignKey("ScopeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
