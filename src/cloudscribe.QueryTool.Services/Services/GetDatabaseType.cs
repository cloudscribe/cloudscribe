using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public DatabaseType GetDatabaseType()
        {
            DatabaseType dbType = new DatabaseType();

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                if(connection.GetType().Name == "SqliteConnection")
                {
                    dbType.isSQLite = true;
                }
                else if(connection.GetType().Name == "SqlConnection")
                {
                    dbType.isSqlServer = true;
                }
                else if(connection.GetType().Name == "MySqlConnection")
                {
                    dbType.isMySql = true;
                }
                else if(connection.GetType().Name == "NpgsqlConnection")
                {
                    dbType.isPostgreSql = true;
                }
            }

            return dbType;

        }
    }
}