// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2016-01-06
// 

using cloudscribe.Core.Models.Setup;
using cloudscribe.Setup.Web;
using cloudscribe.DbHelpers;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Setup.MSSQL
{
    public class DbSetup : IDbSetup
    {
        public DbSetup(
            ILoggerFactory loggerFactory,
            IOptions<MSSQLConnectionOptions> connectionOptions,
            IVersionProviderFactory versionProviderFactory)
        {
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (connectionOptions == null) { throw new ArgumentNullException(nameof(connectionOptions)); }
            if (versionProviderFactory == null) { throw new ArgumentNullException(nameof(versionProviderFactory)); }

            versionProviders = versionProviderFactory;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(DbSetup).FullName);
 
            readConnectionString = connectionOptions.Value.ReadConnectionString;
            writeConnectionString = connectionOptions.Value.WriteConnectionString;
            ownerPrefix = connectionOptions.Value.OwnerPrefix;

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
        }

        private IVersionProviderFactory versionProviders;
        private ILoggerFactory logFactory;
        private ILogger log;
        private string writeConnectionString;
        private string readConnectionString;
        private string ownerPrefix;
        private AdoHelper AdoHelper;

        #region IDbSetup

        public string DBPlatform { get { return "MSSQL"; } }

        public IVersionProviderFactory VersionProviders
        {
            get { return versionProviders; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void EnsureDatabase()
        {
            //logic added by Luis Silva 2012-06-13
            // only works if connection string has been configured and if you add this to user.config or Webc.onfig:
            // <add key="TryToCreateMsSqlDatabase" value="true"/>

            SqlConnection connection = new SqlConnection(writeConnectionString);
            SqlConnection MasterConnection = new SqlConnection();

            try
            {
                SqlConnectionStringBuilder master = new SqlConnectionStringBuilder();
                master.ConnectionString = connection.ConnectionString;
                master["server"] = connection.DataSource;
                master["database"] = "master";

                MasterConnection = new SqlConnection(master.ConnectionString);

                StringBuilder sql = new StringBuilder();
                sql.Append("IF not EXISTS (SELECT name FROM sys.databases WHERE name = @Name) CREATE DATABASE ");
                sql.Append(connection.Database);
                sql.Append(" else select 1");

                SqlCommand command = new SqlCommand(sql.ToString(), MasterConnection);
                command.Parameters.AddWithValue("@Name", connection.Database);
                MasterConnection.Open();
                int res = command.ExecuteNonQuery();

                if (res == -1)
                { log.LogInformation("Successfully created MS SQL Database"); }
                else
                { log.LogInformation("Failed to Create MS SQL Database"); }

            }
            catch (Exception ex)
            {
                log.LogError("Failed to Create MS SQL Database", ex);
            }
            finally
            {
                if (MasterConnection.State == ConnectionState.Open)
                    MasterConnection.Close();
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
                connection = new SqlConnection(readConnectionString);
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
                RunScript(sqlCommand.ToString(), writeConnectionString);
                result = true;
            }
            catch
            {
                result = false;
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
                connection = new SqlConnection(readConnectionString);
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


            if (ownerPrefix != "[dbo].")
            {
                script = script.Replace("[dbo].", ownerPrefix);
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
                connection = new SqlConnection(writeConnectionString);
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
                log.LogError("dbPortal.RunScript failed", ex);
                log.LogInformation("last script statement was " + currentStatement);
                throw;
            }
            finally
            {
                connection.Close();

            }

            return result;
        }

        public bool TableExists(string tableName)
        {
            try
            {
                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT * ");
                sqlCommand.Append("FROM " + tableName + " ");

                using (DbDataReader reader = AdoHelper.ExecuteReader(
                    readConnectionString,
                    CommandType.Text,
                    sqlCommand.ToString(),
                    null))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    // need to return true here if no error
                    // the table may be empty but it does exist if no error
                    return true;
                }
            }
            catch { }

            return false;
        }


        public bool SchemaTableExists()
        {
            return TableExists("mp_SchemaVersion");
        }

        public Guid GetOrGenerateSchemaApplicationId(string applicationName)
        {
            IVersionProvider versionProvider = versionProviders.Get(applicationName);
            if (versionProvider != null) { return versionProvider.ApplicationId; }

            Guid appID = Guid.NewGuid();

            try
            {
                using (DbDataReader reader = GetSchemaId(applicationName))
                {
                    if (reader.Read())
                    {
                        appID = new Guid(reader["ApplicationID"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError("error", ex);
            }

            return appID;

        }

        
        public Version GetSchemaVersion(Guid applicationId)
        {

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;

            try
            {
                using (DbDataReader reader = GetSchemaVersionFromGuid(applicationId))
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

        public bool SchemaVersionExists(Guid applicationId)
        {
            bool result = false;

            using (DbDataReader reader = GetSchemaVersionFromGuid(applicationId))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public bool AddSchemaVersion(
          Guid applicationId,
          string applicationName,
          int major,
          int minor,
          int build,
          int revision)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_SchemaVersion_Insert",
                6);

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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_SchemaVersion_Update",
                6);

            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
            sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
            sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
            sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
            sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        public async Task<DbDataReader> SchemaVersionGetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SchemaVersion_SelectAll", 
                0
                );
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        #endregion


        #region Private methods

        private DbDataReader GetSchemaId(string applicationName)
        {
            return GetReader(
                readConnectionString,
                "mp_SchemaVersion",
                " WHERE LOWER(ApplicationName) = '" + applicationName.ToLower() + "'");
        }

        private DbDataReader GetReader(
            string connectionString,
            string tableName,
            string whereClause)
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


        private DbDataReader GetSchemaVersionFromGuid(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_SchemaVersion_SelectOne",
                1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        #endregion










        //public bool UpdateTableField(
        //    string connectionString,
        //    string tableName,
        //    string keyFieldName,
        //    string keyFieldValue,
        //    string dataFieldName,
        //    string dataFieldValue,
        //    string additionalWhere)
        //{
        //    bool result = false;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE " + tableName + " ");
        //    sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append("  ");

        //    SqlParameter[] arParams = new SqlParameter[1];

        //    arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    //SqlConnection connection = new SqlConnection(connectionString);
        //    //connection.Open();
        //    //try
        //    //{
        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(), arParams);

        //    result = (rowsAffected > 0);

        //    //}
        //    //finally
        //    //{
        //    //    connection.Close();
        //    //}

        //    return result;

        //}

        //public bool UpdateTableField(
        //    string tableName,
        //    string keyFieldName,
        //    string keyFieldValue,
        //    string dataFieldName,
        //    string dataFieldValue,
        //    string additionalWhere)
        //{
        //    bool result = false;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE " + tableName + " ");
        //    sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append("  ");

        //    SqlParameter[] arParams = new SqlParameter[1];

        //    arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    //SqlConnection connection = new SqlConnection(GetConnectionString());
        //    //connection.Open();
        //    //try
        //    //{
        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(), arParams);

        //    result = (rowsAffected > 0);

        //    //}
        //    //finally
        //    //{
        //    //    connection.Close();
        //    //}

        //    return result;

        //}

        

        //public DbDataReader GetReader(
        //    string connectionString,
        //    string query
        //    )
        //{
        //    if (string.IsNullOrEmpty(connectionString)) { connectionString = readConnectionString; }

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        CommandType.Text,
        //        query);

        //}

        //public int ExecteNonQuery(
        //    string connectionString,
        //    string query
        //    )
        //{
        //    if (string.IsNullOrEmpty(connectionString)) { connectionString = writeConnectionString; }

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        query);

        //    return rowsAffected;

        //}




        //public int ExistingSiteCount()
        //{
        //    int count = 0;
        //    try
        //    {
        //        SqlParameterHelper sph = new SqlParameterHelper(
        //            logFactory,
        //            readConnectionString, 
        //            "mp_Sites_CountOtherSites", 
        //            1);

        //        sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, -1);
        //        count = Convert.ToInt32(sph.ExecuteScalar());
        //    }
        //    catch (DbException) { }
        //    catch (InvalidOperationException) { }

        //    return count;

        //}



        //public bool DeleteSchemaVersion(Guid applicationId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Delete", 1);
        //    sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}



        //public IDataReader SchemaVersionGetNonCore()
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectNonCore", 0);
        //    return sph.ExecuteReader();
        //}

        //public int AddSchemaScriptHistory(
        //    Guid applicationId,
        //    string scriptFile,
        //    DateTime runTime,
        //    bool errorOccurred,
        //    string errorMessage,
        //    string scriptBody)
        //{

        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_SchemaScriptHistory_Insert", 
        //        6);
        //    sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
        //    sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
        //    sph.DefineSqlParameter("@RunTime", SqlDbType.DateTime, ParameterDirection.Input, runTime);
        //    sph.DefineSqlParameter("@ErrorOccurred", SqlDbType.Bit, ParameterDirection.Input, errorOccurred);
        //    sph.DefineSqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, errorMessage);
        //    sph.DefineSqlParameter("@ScriptBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, scriptBody);
        //    int newID = Convert.ToInt32(sph.ExecuteScalar());
        //    return newID;
        //}

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

