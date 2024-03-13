using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<List<SavedQuery>> GetSavedQueriesAsync()
        {
            using(var db = _dbContextFactory.CreateContext())
            {
                var savedQueries = await db.SavedQueries.ToListAsync();
                return savedQueries;
            }
        }
    }
}