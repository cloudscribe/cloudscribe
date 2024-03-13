using System.Data;
using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text.RegularExpressions;

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
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            var table = new DataTable();

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
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
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            var table = new DataTable();

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
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

        public async Task<DataTable> RawQueryAsync(DbConnection connection, string query, Dictionary<string,string?> parameters)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            var table = new DataTable();

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = "@" + item.Key;
                        p.DbType = DbType.String;
                        if (item.Value == null) p.Value = DBNull.Value;
                        else p.Value = item.Value;
                        cmd.Parameters.Add(p);
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
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            int rows = 0;

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
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
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            int rows = 0;

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
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

        public async Task<int> RawNonQueryAsync(DbConnection connection, string query, Dictionary<string,string?> parameters)
        {
            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            var dbFactory = DbProviderFactories.GetFactory(connection) ?? throw new Exception("dbFactory is null");
            int rows = 0;

            using (var cmd = dbFactory.CreateCommand())
            {
                if(cmd == null) throw new Exception("cmd is null");
                cmd.Connection = connection;
                cmd.Connection.ConnectionString = connectionString;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = "@" + item.Key;
                        p.DbType = DbType.String;
                        if (item.Value == null) p.Value = DBNull.Value;
                        else p.Value = item.Value;
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.Connection.Open();
                rows = await cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
            }

            return rows;
        }

        /// <summary>
        /// Extracts the needed parameters from the SQL query to a unique list using a regex and default the values to null
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A dictionary of SQL parameters</returns>
        public Dictionary<string, string?> ExtractParametersFromQueryString(string query)
        {
            var parameters = new Dictionary<string, string?>();
            //extract the needed parameters from the query to a unique list using a regex.
            //Required parameters start with a @ symbol and contain alphanumeric characters only.
            var regex = new Regex(@"@([a-z0-9_]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            var matches = regex.Matches(query);
            var neededParameters = new List<string>();
            foreach(Match m in matches)
            {
                if(neededParameters.Contains(m.Value)) continue; //ignore duplicate parameters in the SQL query
                neededParameters.Add(m.Value);
            }

            foreach(var p in neededParameters)
            {
                var key = p.TrimStart('@');
                parameters.Add(key, null); //default all needed parameters to null
            }
            return parameters;
        }
    }
}