using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace cloudscribe.DbHelpers.SQLite
{
    public class SqliteProviderFactory 
    {
        public SqliteProviderFactory()
        {

        }

        //public  bool CanCreateDataSourceEnumerator
        //{
        //    get { return false; }
        //}

        public SqliteCommand CreateCommand()
        {
            return new SqliteCommand();
        }

        //public  DbCommandBuilder CreateCommandBuilder()
        //{
        //    return null;
        //}

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection();
        }

        //public  DbConnectionStringBuilder CreateConnectionStringBuilder()
        //{
        //    return new SqliteConnectionStringBuilder();
        //}

        //public  DbDataAdapter CreateDataAdapter()
        //{
        //    return null;
        //}

        //public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
        //{
        //    return null;
        //}

        public SqliteParameter CreateParameter()
        {
            return new SqliteParameter();
        }
    }
}
