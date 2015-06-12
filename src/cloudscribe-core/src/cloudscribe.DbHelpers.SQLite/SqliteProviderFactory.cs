using Microsoft.Data.SQLite;
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

        public SQLiteCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        //public  DbCommandBuilder CreateCommandBuilder()
        //{
        //    return null;
        //}

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection();
        }

        //public  DbConnectionStringBuilder CreateConnectionStringBuilder()
        //{
        //    return new SQLiteConnectionStringBuilder();
        //}

        //public  DbDataAdapter CreateDataAdapter()
        //{
        //    return null;
        //}

        //public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
        //{
        //    return null;
        //}

        public SQLiteParameter CreateParameter()
        {
            return new SQLiteParameter();
        }
    }
}
