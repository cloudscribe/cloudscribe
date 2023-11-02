using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            int rows = 0;

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                rows = await RawNonQueryAsync(connection, query);
            }

            return rows;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string,string?> parameters)
        {
            int rows = 0;

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                rows = await RawNonQueryAsync(connection, query, parameters);
            }

            return rows;
        }
    }
}