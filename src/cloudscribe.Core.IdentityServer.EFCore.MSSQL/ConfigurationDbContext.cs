// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL
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
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.Client).HasKey(x => x.Id);
                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);
                entity.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ClientName).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ClientUri).HasMaxLength(2000);
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


            modelBuilder.Entity<ScopeClaim>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ScopeClaim).HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<ScopeSecret>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.ScopeSecrets).HasKey(x => x.Id);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.Type).HasMaxLength(250);
                entity.Property(x => x.Value).HasMaxLength(250);
            });
            
            modelBuilder.Entity<Scope>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.Scope).HasKey(x => x.Id);
                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DisplayName).HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.ClaimsRule).HasMaxLength(200);
                entity.HasMany(x => x.Claims).WithOne(x => x.Scope).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.ScopeSecrets).WithOne(x => x.Scope).IsRequired().OnDelete(DeleteBehavior.Cascade);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}