using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.QueryTool.EFCore.Common
{
    public interface IQueryToolDbContext : IDisposable
    {
        DbSet<SavedQuery> SavedQueries { get; }

        DbSet<QueryResult> QueryResults { get; }

        DatabaseFacade Database { get; }
    }
}