// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2004-08-03
// Last Modified:		    2016-01-06

using cloudscribe.Core.Models.Setup;
using cloudscribe.Setup.Web;
using cloudscribe.DbHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Setup.Sqlite
{
    public class DbSetup : IDbSetup
    {

        public DbSetup(
            cloudscribe.DbHelpers.SQLite.SqliteConnectionstringResolver connectionStringResolver,
            ILogger<DbSetup> logger,
            IVersionProviderFactory versionProviderFactory)
        {
            if (connectionStringResolver == null) { throw new ArgumentNullException(nameof(connectionStringResolver)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (versionProviderFactory == null) { throw new ArgumentNullException(nameof(versionProviderFactory)); }

            versionProviders = versionProviderFactory;
            log = logger;
            connectionString = connectionStringResolver.Resolve();
            sqliteFilePath = connectionStringResolver.SqliteFilePath;

            // possibly will change this later to have SqliteFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqliteFactory.Instance);

        }

        private IVersionProviderFactory versionProviders;
        private ILogger log;
        private string connectionString;
        private string sqliteFilePath = string.Empty;
        private AdoHelper AdoHelper;

        #region IDbSetup

        public string DBPlatform
        {
            get { return "SQLite"; }
        }

        public IVersionProviderFactory VersionProviders
        {
            get { return versionProviders; }
        }

        private object theLock = new object();

        public void EnsureDatabase()
        {
            try
            {
                if (sqliteFilePath.Length > 0)
                {


                    //sqlite will automatically create the db when it tries to connect but the folder must exist

                    string folderPath = Path.GetDirectoryName(sqliteFilePath);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    if (!File.Exists(sqliteFilePath))
                    {
                        using (SqliteConnection conn = new SqliteConnection(connectionString))
                        {
                            conn.Open();
                        } //this closes it


                    }



                }
            }
            catch (Exception ex)
            {
                log.LogError("SqlCe database file is not present, tried to create it but this error occurred.", ex);

            }

        }

        public bool CanAccessDatabase()
        {
            return CanAccessDatabase(null);
        }

        public bool CanAccessDatabase(string overrideConnectionInfo)
        {
            bool result = false;

            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqliteConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqliteConnection(connectionString);
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
            sqlCommand.Append(@"
                CREATE TABLE `mp_Testdb` (
                  `FooID` INTEGER PRIMARY KEY,
                  `Foo` varchar(255) NOT NULL default ''
                );
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
            catch (Exception ex)
            {
                if (ex is DbException)
                {
                    //string foo = "foo";
                }
                result = false;
            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE mp_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;");

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


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE mp_Testdb;");

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
            bool result = true;
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(" CREATE TEMPORARY TABLE Temptest ");
            sqlCommand.Append("(IndexID INT  ,");
            sqlCommand.Append(" foo VARCHAR (100) );");
            sqlCommand.Append(" DROP TABLE Temptest;");
            try
            {
                RunScript(sqlCommand.ToString(), connectionString);
            }
            catch
            {
                result = false;
            }


            return result;
        }

        public DbException GetConnectionError(string overrideConnectionInfo)
        {
            DbException exception = null;

            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqliteConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqliteConnection(connectionString);
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

            bool result = false;
            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqliteConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqliteConnection(connectionString);
            }

            connection.Open();

            SqliteTransaction transaction = (SqliteTransaction)connection.BeginTransaction();

            try
            {
                // 2015/11/10 was throwing exception here
                //{"Execute requires the command to have a transaction object when the connection assigned 
                // to the command is in a pending local transaction.  The Transaction property of the command has not been initialized."}

                //AdoHelper.ExecuteNonQuery(connection, script, null);
                AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, script, null);
                transaction.Commit();
                result = true;
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
                sqlCommand.Append("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = :TableName; ");

                SqliteParameter[] arParams = new SqliteParameter[1];

                arParams[0] = new SqliteParameter(":TableName", DbType.String);
                arParams[0].Value = tableName;

                object result = AdoHelper.ExecuteScalar(connectionString,
                    sqlCommand.ToString(),
                    arParams);

                int count = Convert.ToInt32(result);

                return count > 0;

                
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaVersion (");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ApplicationName, ");
            sqlCommand.Append("Major, ");
            sqlCommand.Append("Minor, ");
            sqlCommand.Append("Build, ");
            sqlCommand.Append("Revision )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ApplicationID, ");
            sqlCommand.Append(":ApplicationName, ");
            sqlCommand.Append(":Major, ");
            sqlCommand.Append(":Minor, ");
            sqlCommand.Append(":Build, ");
            sqlCommand.Append(":Revision );");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new SqliteParameter(":ApplicationName", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqliteParameter(":Major", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqliteParameter(":Minor", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqliteParameter(":Build", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqliteParameter(":Revision", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
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
            sqlCommand.Append("ApplicationName = :ApplicationName, ");
            sqlCommand.Append("Major = :Major, ");
            sqlCommand.Append("Minor = :Minor, ");
            sqlCommand.Append("Build = :Build, ");
            sqlCommand.Append("Revision = :Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = :ApplicationID ;");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new SqliteParameter(":ApplicationName", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqliteParameter(":Major", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqliteParameter(":Minor", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqliteParameter(":Build", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqliteParameter(":Revision", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        //disable warning about not really being async
        // we know it is not, and for Sqlite there is probably no benefit to making it really async
#pragma warning disable 1998
        public async Task<DbDataReader> SchemaVersionGetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            
            sqlCommand.Append("ORDER BY ApplicationName ");
            sqlCommand.Append(";");
            
            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);

        }

#pragma warning restore 1998

        #endregion


        #region Private Methods

        private DbDataReader GetSchemaVersionFromGuid(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = :ApplicationID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        private DbDataReader GetSchemaId(string applicationName)
        {
            //return GetReader(
            //    connectionString,
            //    "mp_SchemaVersion",
            //    " WHERE LOWER(ApplicationName) = '" + applicationName.ToLower() + "'");

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LOWER(ApplicationName) = :ApplicationName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ApplicationName", DbType.String);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationName.ToLowerInvariant();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

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
        //    sqlCommand.Append(" SET " + dataFieldName + " = :fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append(" ; ");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":fieldValue", DbType.String);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    SqliteConnection connection = new SqliteConnection(connectionString);
        //    connection.Open();
        //    try
        //    {
        //        int rowsAffected = AdoHelper.ExecuteNonQuery(connection, sqlCommand.ToString(), arParams);
        //        result = (rowsAffected > 0);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

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
        //    sqlCommand.Append(" SET " + dataFieldName + " = :fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append(" ; ");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":fieldValue", DbType.String);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    SqliteConnection connection = new SqliteConnection(connectionString);
        //    connection.Open();
        //    try
        //    {
        //        int rowsAffected = AdoHelper.ExecuteNonQuery(connection, sqlCommand.ToString(), arParams);
        //        result = (rowsAffected > 0);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return result;

        //}

        //public DbDataReader GetReader(
        //    string connectionString,
        //    string tableName,
        //    string whereClause)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * ");
        //    sqlCommand.Append("FROM " + tableName + " ");
        //    sqlCommand.Append(whereClause);
        //    sqlCommand.Append(" ; ");

        //    return AdoHelper.ExecuteReader(
        //        connectionString,
        //        sqlCommand.ToString());

        //}

        //public DbDataReader GetReader(
        //    string connectString,
        //    string query
        //    )
        //{
        //    if (string.IsNullOrEmpty(connectString)) { connectString = connectionString; }

        //    return AdoHelper.ExecuteReader(
        //        connectString,
        //        query);

        //}

        //public int ExecteNonQuery(
        //    string connectString,
        //    string query
        //    )
        //{
        //    if (string.IsNullOrEmpty(connectString)) { connectString = connectionString; }

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectString,
        //        query);

        //    return rowsAffected;

        //}

        //public DataTable GetTable(
        //    String connectionString,
        //    String tableName,
        //    String whereClause)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * ");
        //    sqlCommand.Append("FROM " + tableName + " ");
        //    sqlCommand.Append(whereClause);
        //    sqlCommand.Append(" ; ");

        //    DataSet ds = AdoHelper.ExecuteDataset(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString());

        //    return ds.Tables[0];

        //}


        //public int ExistingSiteCount()
        //{
        //    int count = 0;
        //    try
        //    {
        //        StringBuilder sqlCommand = new StringBuilder();
        //        sqlCommand.Append("SELECT  Count(*) ");
        //        sqlCommand.Append("FROM	mp_Sites ");
        //        sqlCommand.Append(";");

        //        count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //            connectionString,
        //            sqlCommand.ToString(),
        //            null));
        //    }
        //    catch (DbException) { }
        //    catch (InvalidOperationException) { }


        //    return count;

        //}

        


        

        


        



        


        //public bool DeleteSchemaVersion(
        //    Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = :ApplicationID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();


        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(), 
        //        sqlCommand.ToString(), 
        //        arParams);
        //    return (rowsAffected > 0);

        //}

        

        

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
        //        sqlCommand.ToString(),
        //        null);

        //}

        //public int AddSchemaScriptHistory(
        //    Guid applicationId,
        //    string scriptFile,
        //    DateTime runTime,
        //    bool errorOccurred,
        //    string errorMessage,
        //    string scriptBody)
        //{
        //    #region Bit Conversion

        //    int intErrorOccurred;
        //    if (errorOccurred)
        //    {
        //        intErrorOccurred = 1;
        //    }
        //    else
        //    {
        //        intErrorOccurred = 0;
        //    }


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_SchemaScriptHistory (");
        //    sqlCommand.Append("ApplicationID, ");
        //    sqlCommand.Append("ScriptFile, ");
        //    sqlCommand.Append("RunTime, ");
        //    sqlCommand.Append("ErrorOccurred, ");
        //    sqlCommand.Append("ErrorMessage, ");
        //    sqlCommand.Append("ScriptBody )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":ApplicationID, ");
        //    sqlCommand.Append(":ScriptFile, ");
        //    sqlCommand.Append(":RunTime, ");
        //    sqlCommand.Append(":ErrorOccurred, ");
        //    sqlCommand.Append(":ErrorMessage, ");
        //    sqlCommand.Append(":ScriptBody );");

        //    sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

        //    SqliteParameter[] arParams = new SqliteParameter[6];

        //    arParams[0] = new SqliteParameter(":ApplicationID", DbType.String);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new SqliteParameter(":ScriptFile", DbType.String);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    arParams[2] = new SqliteParameter(":RunTime", DbType.DateTime);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = runTime;

        //    arParams[3] = new SqliteParameter(":ErrorOccurred", DbType.Int32);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = intErrorOccurred;

        //    arParams[4] = new SqliteParameter(":ErrorMessage", DbType.Object);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = errorMessage;

        //    arParams[5] = new SqliteParameter(":ScriptBody", DbType.Object);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = scriptBody;


        //    int newID = 0;
        //    newID = Convert.ToInt32(
        //        AdoHelper.ExecuteScalar(
        //        connectionString,
        //        sqlCommand.ToString(),
        //        arParams).ToString());

        //    return newID;

        //}

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = :ID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ID", DbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;


        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(), 
        //        sqlCommand.ToString(), 
        //        arParams);

        //    return (rowsAffected > 0);

        //}

        //public IDataReader GetSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = :ID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ID", DbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public IDataReader GetSchemaScriptHistory(Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = :ApplicationID ");
        //    //sqlCommand.Append("AND ErrorOccurred = 0 ");
        //    sqlCommand.Append(" ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = :ApplicationID ");
        //    sqlCommand.Append("AND ErrorOccurred = 1 ");
        //    sqlCommand.Append(" ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = :ApplicationID ");
        //    sqlCommand.Append("AND ScriptFile = :ScriptFile ");

        //    sqlCommand.Append(" ;");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new SqliteParameter(":ScriptFile", DbType.String, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}



    }
}
