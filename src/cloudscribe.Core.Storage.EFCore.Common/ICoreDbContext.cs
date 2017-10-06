using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public interface ICoreDbContext : IDisposable
    {
        DbSet<GeoCountry> Countries { get; set; }
        DbSet<SiteRole> Roles { get; set; }
        DbSet<SiteHost> SiteHosts { get; set; }
        DbSet<SiteSettings> Sites { get; set; }
        DbSet<GeoZone> States { get; set; }
        DbSet<UserClaim> UserClaims { get; set; }
        DbSet<UserLocation> UserLocations { get; set; }
        DbSet<UserLogin> UserLogins { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<SiteUser> Users { get; set; }
        DbSet<UserToken> UserTokens { get; set; }

        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}