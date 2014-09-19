// Forked From Enterprise Library licensed under Ms-Pl http://www.codeplex.com/entlib
// but implementing a subset of the API from the 2.0 Application Blocks SqlHelper
// using implementation from the newer Ms-Pl version
// Modifications by Joe Audette
// Last Modified 2010-01-28
// 2014-08-26 modified by Joe Audette to use DBProviderFactory so that we can use Glimpse ADO
// for profiling http://blog.simontimms.com/2014/04/21/glimpse-for-raw-ado/
// 2014-08-26 Joe Audette created this version of SqlHelper renamed as AdoHelper and using the more generic
// Db classes via DbProviderFactory, this allows us to do profileing with Glimpse ADO
// Last Modified 2014-09-15


using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace cloudscribe.DbHelpers.SQLite
{
    public static class AdoHelper
    {
        private static DbProviderFactory GetFactory()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
           
            return factory;
        }

       
        private static DbConnection GetConnection(string connectionString)
        {
            var factory = GetFactory();
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            
            return connection;
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static void PrepareCommand(
            DbCommand command,
            DbConnection connection,
            DbTransaction transaction,
            CommandType commandType,
            string commandText,
            DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

            command.CommandType = commandType;
            command.CommandText = commandText;
            command.Connection = connection;

            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            if (commandParameters != null) { AttachParameters(command, commandParameters); }
        }

        private static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        public static int ExecuteNonQuery(
            string connectionString,
            string commandText,
            params DbParameter[] commandParameters)
        { 
            return ExecuteNonQuery(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public static int ExecuteNonQuery(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params DbParameter[] commandParameters)
        {
            int commandTimeout = 30; //30 seconds default http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.commandtimeout.aspx

            return ExecuteNonQuery(connectionString, commandType, commandText, commandTimeout, commandParameters);

           
        }



        public static int ExecuteNonQuery(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            int commandTimeout, 
            params DbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DbProviderFactory factory = GetFactory();

            using (DbConnection connection = GetConnection(connectionString))
            {
                connection.Open();
                using (DbCommand command = factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    command.CommandTimeout = commandTimeout;
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int ExecuteNonQuery(
            DbTransaction transaction,
            CommandType commandType,
            string commandText,
            params DbParameter[] commandParameters)
        {
            int commandTimeout = 30; //30 seconds default

            return ExecuteNonQuery(transaction, commandType, commandText, commandTimeout, commandParameters);

           
        }

        public static int ExecuteNonQuery(
            DbTransaction transaction,
            CommandType commandType,
            string commandText,
            int commandTimeout,
            params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            DbProviderFactory factory = GetFactory();

            using (DbCommand command = factory.CreateCommand())
            {
                PrepareCommand(
                    command,
                    transaction.Connection,
                    transaction,
                    commandType,
                    commandText,
                    commandParameters);

                command.CommandTimeout = commandTimeout;

                return command.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(IDbConnection connection, string commandText, params IDataParameter[] parameters)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = (SQLiteConnection)connection;
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (IDataParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public static DbDataReader ExecuteReader(
            string connectionString,
            string commandText,
            params DbParameter[] commandParameters)
        {
            
            return ExecuteReader(connectionString, CommandType.Text, commandText, commandParameters);


        }

        public static DbDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params DbParameter[] commandParameters)
        {
            int commandTimeout = 30; //30 seconds default
            return ExecuteReader(connectionString, commandType, commandText, commandTimeout, commandParameters);

            
        }

        public static DbDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            int commandTimeout, 
            params DbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DbProviderFactory factory = GetFactory();

            // we cannot wrap this connection in a using
            // we need to let the reader close it at using(IDataReader reader = ...
            // otherwise it gets closed before the reader can use it
            DbConnection connection = null;
            try
            {
                //connection = new SqlConnection(connectionString);
                connection = GetConnection(connectionString);

                connection.Open();
                using (DbCommand command = factory.CreateCommand())
                {
                    PrepareCommand(
                        command,
                        connection,
                        null,
                        commandType,
                        commandText,
                        commandParameters);

                    command.CommandTimeout = commandTimeout;

                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }


            }
            catch
            {
                if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
                throw;
            }
        }


        // Maybe we should implement helper mothds for async?
        // problem is then you need to use connection.OpenAsync, reader.ReadAsync etc
        // and for many scenarios async db access is not the best way

        //public static async Task<DbDataReader> ExecuteReaderAsync(
        //    string connectionString,
        //    CommandType commandType,
        //    string commandText,
        //    int commandTimeout,
        //    params DbParameter[] commandParameters)
        //{
        //    if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        //    DbProviderFactory factory = GetFactory();

        //    // we cannot wrap this connection in a using
        //    // we need to let the reader close it at using(IDataReader reader = ...
        //    // otherwise it gets closed before the reader can use it
        //    DbConnection connection = null;
        //    try
        //    {
        //        //connection = new SqlConnection(connectionString);
        //        connection = GetConnection(connectionString);

        //        connection.Open();
        //        using (DbCommand command = factory.CreateCommand())
        //        {
        //            PrepareCommand(
        //                command,
        //                connection,
        //                null,
        //                commandType,
        //                commandText,
        //                commandParameters);

        //            command.CommandTimeout = commandTimeout;

        //            DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        //            return reader;
        //        }


        //    }
        //    catch
        //    {
        //        if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
        //        throw;
        //    }
        //}

        public static object ExecuteScalar(
            string connectionString,
            string commandText,
            params DbParameter[] commandParameters)
        {
            return ExecuteScalar(connectionString, CommandType.Text, commandText, commandParameters);

        }

        public static object ExecuteScalar(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params DbParameter[] commandParameters)
        {
            int commandTimeout = 30; //30 seconds default
            return ExecuteScalar(connectionString, commandType, commandText, commandTimeout, commandParameters);

          
        }

        public static object ExecuteScalar(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            int commandTimeout, 
            params DbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DbProviderFactory factory = GetFactory();

            using (DbConnection connection = GetConnection(connectionString))
            {
                connection.Open();
                using (DbCommand command = factory.CreateCommand())
                {
                    PrepareCommand(command, connection, (DbTransaction)null, commandType, commandText, commandParameters);
                    command.CommandTimeout = commandTimeout;

                    return command.ExecuteScalar();
                }
            }
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (DbParameter[])null);
        }

        public static DataSet ExecuteDataset(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params DbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DbProviderFactory factory = GetFactory();

            using (DbConnection connection = GetConnection(connectionString))
            {
                connection.Open();
                using (DbCommand command = factory.CreateCommand())
                {
                    PrepareCommand(command, connection, (DbTransaction)null, commandType, commandText, commandParameters);
                    using (DbDataAdapter adpater = factory.CreateDataAdapter())
                    {
                        adpater.SelectCommand = command;
                        DataSet dataSet = new DataSet();
                        adpater.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }
    }
}
