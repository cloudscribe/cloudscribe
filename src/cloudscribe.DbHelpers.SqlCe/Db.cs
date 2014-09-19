// Author:         Joe Audette
// Created:        2010-03-09
// Last Modified   2014-09-19


using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Configuration;
using log4net;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;

namespace cloudscribe.DbHelpers.SqlCe
{
    public class Db : IDb
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Db));


        public Db()
        { }

        public string DBPlatform
        {
            get { return "SqlCe"; }
        }

        public void EnsureDatabase()
        {
            try
            {
                if (ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"] != null)
                {
                    string path 
                        = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"]);
                    
                    string connectionString = "Data Source=" + path + ";Persist Security Info=False;";

                    if (!File.Exists(path))
                    {
                        lock (typeof(Db))
                        {
                            if (!File.Exists(path))
                            {
                                using (SqlCeEngine engine = new SqlCeEngine(connectionString))
                                {
                                    engine.CreateDatabase();
                                }
                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("SqlCe database file is not present, tried to create it but this error occurred.", ex);

            }

        }

        public bool CanAccessDatabase()
        {
            return CanAccessDatabase(null);
        }

        public bool CanAccessDatabase(string overrideConnectionInfo)
        {
            // TODO: FxCop says not to swallow nonspecific exceptions
            // need to find all possible exceptions that could happen here and
            // catch them specifically
            // ultimately we want to return false on any exception

            bool result = false;

            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(ConnectionString.GetConnectionString());
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

        public DbException GetConnectionError(string overrideConnectionInfo)
        {
            DbException exception = null;

            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(ConnectionString.GetConnectionString());
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
                @" 
                CREATE TABLE [mp_Testdb](
                  [FooID] [int] IDENTITY(1,1) NOT NULL,
                  [Foo] [nvarchar](255) NOT NULL,
                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY 
                (
                  [FooID] 
                ) 
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
                @"ALTER TABLE [mp_Testdb] ADD
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
                @"DROP TABLE [mp_Testdb] 
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

            
            return result;

        }

        public bool RunScript(
            FileInfo scriptFile,
            string overrideConnectionInfo)
        {
            if (scriptFile == null) return false;

            string script = File.ReadAllText(scriptFile.FullName);

            if ((script == null) || (script.Length == 0)) return true;

            return RunScript(script, overrideConnectionInfo);

        }


        public bool RunScript(string script, string overrideConnectionInfo)
        {
            if ((script == null) || (script.Length == 0)) return true;

            
            bool result = false;
            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(ConnectionString.GetConnectionString());
            }

            string[] delimiter = new string[] { "GO\r\n" };

            script = script.Replace("GO", "GO\r\n");

            string[] arrSqlStatements = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            connection.Open();

            //string[] subdelimiter = new string[] { ";" };

            SqlCeTransaction transaction = connection.BeginTransaction();
            string currentStatement = string.Empty;
            try
            {
                foreach (string sqlStatement in arrSqlStatements)
                {
                    //string[] subStatements = s.Split(subdelimiter, StringSplitOptions.RemoveEmptyEntries);

                    //foreach (string sqlStatement in subStatements)
                    //{
                    if (sqlStatement.Trim().Length > 0)
                    {
                        currentStatement = sqlStatement;
                        AdoHelper.ExecuteNonQuery(
                            transaction,
                            CommandType.Text,
                            sqlStatement,
                            null);

                    }
                    // }
                }


                transaction.Commit();
                result = true;

            }
            catch (SqlCeException ex)
            {
                transaction.Rollback();
                log.Error("db.RunScript failed", ex);
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
            string connectionString,
            string tableName,
            string keyFieldName,
            string keyFieldValue,
            string dataFieldName,
            string dataFieldValue,
            string additionalWhere)
        {
            bool result = false;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            result = (rowsAffected > 0);



            return result;

        }


        public bool UpdateTableField(
            string tableName,
            string keyFieldName,
            string keyFieldValue,
            string dataFieldName,
            string dataFieldValue,
            string additionalWhere)
        {
            bool result = false;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(), arParams);

            result = (rowsAffected > 0);

            return result;

        }


        public IDataReader GetReader(
            string connectionString,
            string tableName,
            string whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

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
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetConnectionString(); }

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
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetConnectionString(); }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                query);

            return rowsAffected;

        }

        public DataTable GetTable(
            string connectionString,
            string tableName,
            string whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            DataSet ds = AdoHelper.ExecuteDataset(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

            return ds.Tables[0];

        }


        public int ExistingSiteCount()
        {
            int count = 0;
            try
            {
                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT  Count(*) ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append(";");

                count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                    ConnectionString.GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    null));

            }
            catch (DbException) { }
            catch (InvalidOperationException) { }
            catch (Exception)
            {
                //this is a needed hack because SqlCeException does not inherit from DbException like other data layers
                //instead it inherits from System.Exception which we would rather not trap
                
            }

            return count;

        }

        public bool SitesTableExists()
        {
            return TableExists("mp_Sites");
        }

        public bool TableExists(string tableName)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	INFORMATION_SCHEMA.TABLES ");
            sqlCommand.Append("WHERE TABLE_NAME = @TableName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TableName", SqlDbType.NVarChar, 100);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = tableName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
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
            catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException does not inherit from DbException as it should
                
                log.Error(ex);
            }


            return new Version(major, minor, build, revision);
        }

        public Guid GetOrGenerateSchemaApplicationId(string applicationName)
        {

            if (string.Equals(applicationName, "cloudscribe-core", StringComparison.InvariantCultureIgnoreCase))
                return new Guid("b7dcd727-91c3-477f-bc42-d4e5c8721daa");

            if (string.Equals(applicationName, "cloudscribe-cms", StringComparison.InvariantCultureIgnoreCase))
                return new Guid("2ba3e968-dd0b-44cb-9689-188963ed2664");

            string sguid = AppSettings.GetString(applicationName + "_appGuid", string.Empty);
            if (sguid.Length == 36) { return new Guid(sguid); }

            Guid appID = Guid.NewGuid();

            try
            {
                using (IDataReader reader = GetSchemaId(applicationName))
                {
                    if (reader.Read())
                    {
                        appID = new Guid(reader["ApplicationID"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }


            return appID;

        }


        private IDataReader GetSchemaId(string applicationName)
        {
            return GetReader(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaVersion ");
            sqlCommand.Append("(");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ApplicationName, ");
            sqlCommand.Append("Major, ");
            sqlCommand.Append("Minor, ");
            sqlCommand.Append("Build, ");
            sqlCommand.Append("Revision ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ApplicationName, ");
            sqlCommand.Append("@Major, ");
            sqlCommand.Append("@Minor, ");
            sqlCommand.Append("@Build, ");
            sqlCommand.Append("@Revision ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ApplicationName", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqlCeParameter("@Major", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqlCeParameter("@Minor", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqlCeParameter("@Build", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqlCeParameter("@Revision", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SchemaVersion ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ApplicationName = @ApplicationName, ");
            sqlCommand.Append("Major = @Major, ");
            sqlCommand.Append("Minor = @Minor, ");
            sqlCommand.Append("Build = @Build, ");
            sqlCommand.Append("Revision = @Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ApplicationName", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqlCeParameter("@Major", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqlCeParameter("@Minor", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqlCeParameter("@Build", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqlCeParameter("@Revision", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        //public bool DeleteSchemaVersion(Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

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



        private IDataReader GetSchemaVersionFromGuid(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        //public IDataReader SchemaVersionGetNonCore()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaVersion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID <> '077E4857-F583-488E-836E-34A4B04BE855' ");
        //    sqlCommand.Append("ORDER BY ApplicationName ");
        //    sqlCommand.Append(";");

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        null);
        //}

        public int AddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaScriptHistory ");
            sqlCommand.Append("(");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ScriptFile, ");
            sqlCommand.Append("RunTime, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage, ");
            sqlCommand.Append("ScriptBody ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ScriptFile, ");
            sqlCommand.Append("@RunTime, ");
            sqlCommand.Append("@ErrorOccurred, ");
            sqlCommand.Append("@ErrorMessage, ");
            sqlCommand.Append("@ScriptBody ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ScriptFile", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new SqlCeParameter("@RunTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new SqlCeParameter("@ErrorOccurred", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = errorOccurred;

            arParams[4] = new SqlCeParameter("@ErrorMessage", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new SqlCeParameter("@ScriptBody", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;


            int newId = Convert.ToInt32(AdoHelper.DoInsertGetIdentitiy(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            //log.Info("Identity was " + newId.ToString());

            return newId;
        }

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = @ID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);
        //}

        //public IDataReader GetSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = @ID ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);
        //}

        //public IDataReader GetSchemaScriptHistory(Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ");
        //    sqlCommand.Append("ORDER BY ");
        //    sqlCommand.Append("ID  ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);
        //}

        //public IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("ErrorOccurred = 1 ");
        //    sqlCommand.Append("ORDER BY ");
        //    sqlCommand.Append("ID  ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[1];

        //    arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);
        //}

        //public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("ScriptFile = @ScriptFile ");
        //    sqlCommand.Append(";");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId;

        //    arParams[1] = new SqlCeParameter("@ScriptFile", SqlDbType.NVarChar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);
        //}




    }
}
