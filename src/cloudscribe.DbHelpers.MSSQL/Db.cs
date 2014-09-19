using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace cloudscribe.DbHelpers.MSSQL
{
    public class Db : cloudscribe.Core.Models.IDb
    {
        public Db()
        { }

        private static readonly ILog log = LogManager.GetLogger(typeof(Db));

        public string DBPlatform { get { return "MSSQL"; } }


        public bool CanAccessDatabase(String overrideConnectionInfo)
        {
            // TODO: FxCop says not to swallow nonspecific exceptions
            // need to find all possible exceptions that could happen here and
            // catch them specifically
            // ultimately we want to return false on any exception

            bool result = false;

            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlConnection(ConnectionString.GetReadConnectionString());
            }

            try
            {
                connection.Open();
                result = (connection.State == ConnectionState.Open);

            }
            catch { }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }


            return result;

        }

        public DbException GetConnectionError(String overrideConnectionInfo)
        {
            DbException exception = null;

            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlConnection(ConnectionString.GetReadConnectionString());
            }

            try
            {
                connection.Open();


            }
            catch (DbException ex)
            {
                exception = ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }


            return exception;

        }

        public bool CanAlterSchema(string overrideConnectionInfo)
        {

            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            

            sqlCommand.Append(
               @"SET QUOTED_IDENTIFIER ON
                GO
                CREATE TABLE [dbo].[mp_Testdb](
                  [FooID] [int] IDENTITY(1,1) NOT NULL,
                  [Foo] [nvarchar](255) NOT NULL,
                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED 
                (
                  [FooID] ASC
                ) WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
                ) 
                GO
                ");

            try
            {
                RunScript(
                    sqlCommand.ToString(),
                    overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"ALTER TABLE [dbo].[mp_Testdb] ADD
                [MoreFoo] [nvarchar] (255)  NULL
                GO
                ");

            try
            {
                RunScript(
                    sqlCommand.ToString(),
                    overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"DROP TABLE [dbo].[mp_Testdb] 
                GO
                ");

            try
            {
                RunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

           

            return result;

        }

        public bool CanCreateTemporaryTables()
        {
            bool result = false;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE TABLE #Test 
                (
                  IndexID int IDENTITY (1, 1) NOT NULL,
                  UserName nvarchar(50),
                  LoginName nvarchar(50)
                )
                DROP TABLE #Test
                GO
                  
                ");

            try
            {
                RunScript(sqlCommand.ToString(), ConnectionString.GetWriteConnectionString());
                result = true;
            }
            catch
            {
                result = false;
            }


            return result;

        }

        public bool RunScript(
            FileInfo scriptFile,
            String overrideConnectionInfo)
        {
            if (scriptFile == null) return false;

            string script = File.ReadAllText(scriptFile.FullName);

            if ((script == null) || (script.Length == 0)) return true;

            return RunScript(script, overrideConnectionInfo);

        }


        public bool RunScript(string script, string overrideConnectionInfo)
        {
            if ((script == null) || (script.Length == 0)) return true;

            
            if (AppSettings.MSSQLOwnerPrefix != "[dbo].")
            {
                script = script.Replace("[dbo].", AppSettings.MSSQLOwnerPrefix);
            }

            bool result = false;
            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlConnection(ConnectionString.GetWriteConnectionString());
            }

            string[] delimiter = new string[] { "GO\r\n" };

            script = script.Replace("GO", "GO\r\n");

            string[] arrSqlStatements = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();
            string currentStatement = string.Empty;

            try
            {
                foreach (String sqlStatement in arrSqlStatements)
                {
                    if (sqlStatement.Trim().Length > 0)
                    {
                        currentStatement = sqlStatement;
                        AdoHelper.ExecuteNonQuery(
                            transaction,
                            CommandType.Text,
                            sqlStatement,
                            null);

                    }
                }


                transaction.Commit();
                result = true;

            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                log.Error("dbPortal.RunScript failed", ex);
                log.Info("last script statement was " + currentStatement);
                throw;
            }
            finally
            {
                connection.Close();

            }

            return result;
        }

        public bool UpdateTableField(
            String connectionString,
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            bool result = false;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");

            SqlParameter[] arParams = new SqlParameter[1];

            arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            //SqlConnection connection = new SqlConnection(connectionString);
            //connection.Open();
            //try
            //{
            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(), arParams);

            result = (rowsAffected > 0);

            //}
            //finally
            //{
            //    connection.Close();
            //}

            return result;

        }

        public bool UpdateTableField(
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            bool result = false;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");

            SqlParameter[] arParams = new SqlParameter[1];

            arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            //SqlConnection connection = new SqlConnection(GetConnectionString());
            //connection.Open();
            //try
            //{
            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(), arParams);

            result = (rowsAffected > 0);

            //}
            //finally
            //{
            //    connection.Close();
            //}

            return result;

        }

        public IDataReader GetReader(
            String connectionString,
            String tableName,
            String whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append("  ");

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

        }

        public IDataReader GetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                query);

        }

        public int ExecteNonQuery(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                query);

            return rowsAffected;

        }

        public DataTable GetTable(
            String connectionString,
            String tableName,
            String whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append("  ");

            DataSet ds = AdoHelper.ExecuteDataset(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

            return ds.Tables[0];

        }

        public bool TableExists(string tableName)
        {
            //return mojoPortal.Data.Common.DBPortal.DatabaseHelperTableExists(tableName);

            using (SqlConnection connection = new SqlConnection(ConnectionString.GetReadConnectionString()))
            {
                string[] restrictions = new string[4];
                restrictions[2] = tableName;
                connection.Open();
                DataTable table = connection.GetSchema("Tables", restrictions);
                if (table != null)
                {
                    return (table.Rows.Count > 0);
                }
            }

            return false;
        }

        public bool SitesTableExists()
        {
            return TableExists("mp_Sites");
        }



        public Guid GetSchemaApplicationId(string applicationName)
        {
            //if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
            //    return new Guid("077e4857-f583-488e-836e-34a4b04be855");


            Guid appID = Guid.NewGuid();

            using (IDataReader reader = GetSchemaId(applicationName))
            {
                if (reader.Read())
                {
                    appID = new Guid(reader["ApplicationID"].ToString());

                }
            }

            return appID;

        }


        private IDataReader GetSchemaId(string applicationName)
        {
            return GetReader(
                ConnectionString.GetReadConnectionString(),
                "mp_SchemaVersion",
                " WHERE LOWER(ApplicationName) = '" + applicationName.ToLower() + "'");
        }

       
        public bool AddSchemaVersion(
          Guid applicationId,
          string applicationName,
          int major,
          int minor,
          int build,
          int revision)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Insert", 6);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
            sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
            sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
            sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
            sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
            return (rowsAffected > 0);
        }


        public bool UpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Update", 6);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
            sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
            sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
            sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
            sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        //public bool DeleteSchemaVersion(Guid applicationId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Delete", 1);
        //    sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public bool SchemaVersionExists(Guid applicationId)
        {
            bool result = false;

            using (IDataReader reader = GetSchemaVersionFromGuid(applicationId))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }


        public Version GetSchemaVersion(Guid applicationId)
        {
            
            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;

            try
            {
                using (IDataReader reader = GetSchemaVersionFromGuid(applicationId))
                {
                    if (reader.Read())
                    {
                        major = Convert.ToInt32(reader["Major"]);
                        minor = Convert.ToInt32(reader["Minor"]);
                        build = Convert.ToInt32(reader["Build"]);
                        revision = Convert.ToInt32(reader["Revision"]);
                    }
                }

            }
            catch (DbException) { }
            catch (InvalidOperationException) { }
            //catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                //if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                //log.Error(ex);
            }


            return new Version(major, minor, build, revision);
        }

        private IDataReader GetSchemaVersionFromGuid(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectOne", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        //public IDataReader SchemaVersionGetNonCore()
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectNonCore", 0);
        //    return sph.ExecuteReader();
        //}

        public int AddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaScriptHistory_Insert", 6);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
            sph.DefineSqlParameter("@RunTime", SqlDbType.DateTime, ParameterDirection.Input, runTime);
            sph.DefineSqlParameter("@ErrorOccurred", SqlDbType.Bit, ParameterDirection.Input, errorOccurred);
            sph.DefineSqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, errorMessage);
            sph.DefineSqlParameter("@ScriptBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, scriptBody);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaScriptHistory_Delete", 1);
        //    sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public IDataReader GetSchemaScriptHistory(int id)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectOne", 1);
        //    sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
        //    return sph.ExecuteReader();
        //}

        //public IDataReader GetSchemaScriptHistory(Guid applicationId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectByApp", 1);
        //    sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    return sph.ExecuteReader();
        //}

        //public IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectErrorsByApp", 1);
        //    sph.DefineSqlParameter("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    return sph.ExecuteReader();
        //}

        //public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_Exists", 2);
        //    sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
        //    int count = Convert.ToInt32(sph.ExecuteScalar());
        //    return (count > 0);
        //}
    }
}
