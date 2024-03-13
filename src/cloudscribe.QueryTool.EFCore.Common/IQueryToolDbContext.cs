using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.QueryTool.EFCore.Common
{
    public interface IQueryToolDbContext : IDisposable
    {
        DbSet<SavedQuery> SavedQueries { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        DatabaseFacade Database { get; }
    }
}