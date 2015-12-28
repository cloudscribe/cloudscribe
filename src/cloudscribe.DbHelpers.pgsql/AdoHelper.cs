
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.DbHelpers.pgsql
{
    public static class AdoHelper
    {
        private static DbProviderFactory GetFactory()
        {
            //var factory = DbProviderFactories.GetFactory("Npgsql");

            //return factory;
            return Npgsql.NpgsqlFactory.Instance;
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

        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            DbParameter[] commandParameters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int commandTimeout = 30; //30 seconds default

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

                    return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
                }


            }
            catch
            {
                if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
                throw;
            }
        }

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
                connection.Open();
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
