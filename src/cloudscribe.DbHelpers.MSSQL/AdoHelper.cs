
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DbHelpers.MSSQL
{
    public static class AdoHelper
    {
        private static DbProviderFactory GetFactory()
        {
            // back in mvc 5 this DbProviderFactories was configured in web.config
            // and glimpse was able to wrap around it for tracking ado queries
            //var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            // need to figure out how to make this ado code glimpse friendly
            // so that glimpse can track the queries.
            // I see clues how glimpse hooks into EF here: 
            //https://github.com/Glimpse/Glimpse.Prototype/blob/dev/src/Glimpse.Agent.Dnx/Internal/Inspectors/EF/EFDiagnosticsInspector.cs
            // and I've asked a question here:
            // https://github.com/Glimpse/Glimpse.Prototype/issues/99
            // my guess is that we will need to change this class from being static
            // as part of the solution, but need to learn more before making changes in that direction


            return System.Data.SqlClient.SqlClientFactory.Instance;

           // return factory;
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

        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int commandTimeout = 30; //30 seconds default http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.commandtimeout.aspx

            return await ExecuteNonQueryAsync(
                connectionString, 
                commandType, 
                commandText, 
                commandTimeout, 
                commandParameters,
                cancellationToken);


        }

        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            int commandTimeout,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
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
                    return await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(
            DbTransaction transaction,
            CommandType commandType,
            string commandText,
            int commandTimeout,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
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

                return await command.ExecuteNonQueryAsync(cancellationToken);
            }
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



        public static async Task<DbDataReader> ExecuteReaderAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int commandTimeout = 30; //30 seconds default
            return await ExecuteReaderAsync(
                connectionString, 
                commandType, 
                commandText, 
                commandTimeout, 
                commandParameters,
                cancellationToken);


        }

        public static async Task<DbDataReader> ExecuteReaderAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            int commandTimeout,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
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

                    DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);

                    return reader;
                }


            }
            catch
            {
                if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
                throw;
            }
        }

        //public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, CommandType cmdType,
        //string cmdText, params SqlParameter[] commandParameters)
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        using (var command = new SqlCommand(cmdText, connection))
        //        {
        //            try
        //            {
        //                command.CommandType = cmdType;
        //                command.Parameters.AddRange(commandParameters);
        //                connection.Open();
        //                return await command.ExecuteReaderAsync();
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

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

        public static async Task<object> ExecuteScalarAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int commandTimeout = 30; //30 seconds default
            return await ExecuteScalarAsync(
                connectionString, 
                commandType, 
                commandText, 
                commandTimeout, 
                commandParameters,
                cancellationToken);


        }

        public static async Task<object> ExecuteScalarAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            int commandTimeout,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            DbProviderFactory factory = GetFactory();

            using (DbConnection connection = GetConnection(connectionString))
            {
                await connection.OpenAsync();
                using (DbCommand command = factory.CreateCommand())
                {
                    PrepareCommand(command, connection, (DbTransaction)null, commandType, commandText, commandParameters);
                    command.CommandTimeout = commandTimeout;

                    return await command.ExecuteScalarAsync(cancellationToken);
                }
            }
        }

    }
}
