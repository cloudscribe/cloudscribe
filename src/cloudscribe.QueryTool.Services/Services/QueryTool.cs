using System.Data;
using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public QueryTool(
            IQueryToolDbContextFactory dbContextFactory,
            IConfiguration config,
            ILogger<QueryTool> logger
            )
        {
            _dbContextFactory = dbContextFactory;
            _log = logger;
            _config = config;
        }
        private readonly IQueryToolDbContextFactory _dbContextFactory;
        private readonly ILogger _log;
        private readonly IConfiguration _config;

        public async Task<DataTable> RawQueryAsync(DbConnection connection, string query)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection);
            var table = new DataTable();

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection.Open();
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                table.Load(reader);
                cmd.Connection.Close();
            }

            return table;
        }

        public async Task<DataTable> RawQueryAsync(DbConnection connection, string query, params DbParameter[] parameters)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection);
            var table = new DataTable();

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.Connection.Open();
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                table.Load(reader);
                cmd.Connection.Close();
            }

            return table;
        }

        public async Task<int> RawNonQueryAsync(DbConnection connection, string query)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection);
            int rows = 0;

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection.Open();
                rows = await cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
            }

            return rows;
        }

        public async Task<int> RawNonQueryAsync(DbConnection connection, string query, params DbParameter[] parameters)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection);
            int rows = 0;

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.Connection.Open();
                rows = await cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
            }

            return rows;
        }
    }
}