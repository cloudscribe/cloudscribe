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
            await Task.Run(() => {
                using(var db = _dbContextFactory.CreateContext())
                {
                    DbConnection connection = db.Database.GetDbConnection();
                    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            var rc = adapter.Fill(0, 0, dataTable);
                        }
                    }
                }
            });
            return dataTable;
        }
    }
}