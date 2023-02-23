using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using(var db = _dbContextFactory.CreateContext())
            {
            return await db.Database.ExecuteSqlRawAsync(query);
            }
        }
    }
}