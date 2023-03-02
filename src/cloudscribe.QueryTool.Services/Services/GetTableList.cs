using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<DataTable> GetTableList()
        {
            DataTable dataTable = new DataTable();

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                string? query = null;
                string? provider = db.Database.ProviderName;

                if(!string.IsNullOrWhiteSpace(provider))
                {
                    provider = provider.ToLower();

                    if(provider.EndsWith("sqlite"))
                    {
                        query = "SELECT name AS TableName FROM sqlite_master WHERE type='table' ORDER BY name;";
                        // dataTable = RawQuery<System.Data.SQLite.SQLiteConnection>(query);
                    }
                    else if(provider.EndsWith("sqlserver"))
                    {
                        query = "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;";
                        // dataTable = RawQuery<System.Data.SqlClient.SqlConnection>(query);
                    }
                    else if(provider.EndsWith("mysql"))
                    {
                        query = "SELECT table_name AS TableName FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = database() ORDER BY table_name;";
                    }
                    else if(provider.EndsWith("postgresql"))
                    {
                        query = "SELECT table_name AS TableName FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
                    }
                }

                if(string.IsNullOrWhiteSpace(query))
                {
                    throw new Exception("Unsupported database type");
                }

                dataTable = await RawQueryAsync(connection, query);
            }

            return dataTable;
        }
    }
}