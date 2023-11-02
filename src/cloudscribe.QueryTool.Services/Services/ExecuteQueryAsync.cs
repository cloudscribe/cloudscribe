using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            DataTable dataTable = new DataTable();

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                dataTable = await RawQueryAsync(connection, query);
            }

            return dataTable;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string,string?> parameters)
        {
            DataTable dataTable = new DataTable();

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                dataTable = await RawQueryAsync(connection, query, parameters);
            }

            return dataTable;
        }
    }
}