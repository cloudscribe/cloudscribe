// Original Author:					Joseph Hill
// Created:							2005-02-16 
// Additions and fixes have been added by Joe Audette, Dean Brettle, TJ Fontaine
// Last Modified:                   2016-01-06


using cloudscribe.Core.Models.Setup;
using cloudscribe.Setup.Web;
using cloudscribe.DbHelpers;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Setup.pgsql
{
    public class DbSetup : IDbSetup
    {
        public DbSetup(
            ILoggerFactory loggerFactory,
            IOptions<ConnectionStringOptions> configuration,
            IVersionProviderFactory versionProviderFactory)
        {
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (versionProviderFactory == null) { throw new ArgumentNullException(nameof(versionProviderFactory)); }

            versionProviders = versionProviderFactory;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(DbSetup).FullName);

            writeConnectionString = configuration.Value.WriteConnectionString;
            readConnectionString = configuration.Value.ReadConnectionString;

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);

        }

        private IVersionProviderFactory versionProviders;
        private ILoggerFactory logFactory;
        private ILogger log;
        private string writeConnectionString;
        private string readConnectionString;
        private AdoHelper AdoHelper;

        #region IDb

        public string DBPlatform
        {
            get { return "pgsql"; }
        }

        public IVersionProviderFactory VersionProviders
        {
            get { return versionProviders; }
        }

        public void EnsureDatabase()
        {
            //not applicable for this platform

        }

        public bool CanAccessDatabase()
        {
            return CanAccessDatabase(null);
        }


        public bool CanAccessDatabase(string overrideConnectionInfo)
        {
            bool result = false;

            NpgsqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new NpgsqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new NpgsqlConnection(writeConnectionString);
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

            sqlCommand.Append("CREATE SEQUENCE \"mp_testdb_fooid_seq\"; CREATE TABLE \"mp_testdb\" ( \"categoryid\"	int4 NOT NULL DEFAULT nextval('\"mp_testdb_fooid_seq\"'::text), \"fooid\" int4 NOT NULL, \"foo\" varchar(255) NULL );");

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
            //sqlCommand.Append("BEGIN; LOCK mp_testdb; ALTER TABLE mp_testdb ADD COLUMN morefoo character varying(255);  COMMIT;");
            sqlCommand.Append("ALTER TABLE mp_testdb ADD COLUMN morefoo character varying(255);");

            try
            {
                RunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException dbx)
            {
                log.LogError("error in can alter shcema", dbx);
                result = false;
            }
            catch (ArgumentException agx)
            {
                log.LogError("error in can alter shcema", agx);
                result = false;
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE \"mp_testdb\"; DROP SEQUENCE \"mp_testdb_fooid_seq\" ;");

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
            sqlCommand.Append(" create temp table t_temptabletest ");
            sqlCommand.Append("(id int, pagename varchar (100)); ");

            sqlCommand.Append("drop table t_temptabletest;");
            try
            {
                RunScript(sqlCommand.ToString(), writeConnectionString);
            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }

        public DbException GetConnectionError(string overrideConnectionInfo)
        {
            DbException exception = null;

            NpgsqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new NpgsqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new NpgsqlConnection(writeConnectionString);
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
            NpgsqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new NpgsqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new NpgsqlConnection(writeConnectionString);
            }

            connection.Open();


            NpgsqlTransaction transaction = connection.BeginTransaction();

            try
            {
                AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, script);
                transaction.Commit();
                result = true;

            }
            catch (NpgsqlException ex)
            {
                transaction.Rollback();
                log.LogError("dbPortal.RunScript failed", ex);
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
            //NpgsqlConnection connection = new NpgsqlConnection(ConnectionString.GetWriteConnectionString());
            //string[] restrictions = new string[4];
            //restrictions[2] = tableName;
            //connection.Open();
            //DataTable table = connection.GetSchema("Tables", restrictions);
            //connection.Close();
            //if (table != null)
            //{
            //    return (table.Rows.Count > 0);
            //}

            //return false;

            try
            {
                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT * ");
                sqlCommand.Append("FROM " + tableName + "; ");

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
                }
                // if no error table exists
                return true;
            }
            catch { }

            return false;
        }

        public bool SchemaTableExists()
        {
            return TableExists("mp_schemaversion");
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new NpgsqlParameter("applicationname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new NpgsqlParameter("major", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new NpgsqlParameter("minor", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new NpgsqlParameter("build", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new NpgsqlParameter("revision", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_schemaversion (");
            sqlCommand.Append("applicationid, ");
            sqlCommand.Append("applicationname, ");
            sqlCommand.Append("major, ");
            sqlCommand.Append("minor, ");
            sqlCommand.Append("build, ");
            sqlCommand.Append("revision )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":applicationid, ");
            sqlCommand.Append(":applicationname, ");
            sqlCommand.Append(":major, ");
            sqlCommand.Append(":minor, ");
            sqlCommand.Append(":build, ");
            sqlCommand.Append(":revision ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");
            //select cast(currval('mp_schemascripthistoryid_seq') as int4);$_$

            //int rowsAffected = AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_schemaversion_insert(:applicationid,:applicationname,:major,:minor,:build,:revision)",
            //    arParams);

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new NpgsqlParameter("applicationname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new NpgsqlParameter("major", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new NpgsqlParameter("minor", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new NpgsqlParameter("build", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new NpgsqlParameter("revision", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_schemaversion ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("applicationname = :applicationname, ");
            sqlCommand.Append("major = :major, ");
            sqlCommand.Append("minor = :minor, ");
            sqlCommand.Append("build = :build, ");
            sqlCommand.Append("revision = :revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("applicationid = :applicationid ");
            sqlCommand.Append(";");

            //int rowsAffected = AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_schemaversion_update(:applicationid,:applicationname,:major,:minor,:build,:revision)",
            //    arParams);

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<DbDataReader> SchemaVersionGetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_schemaversion ");
            
            sqlCommand.Append("ORDER BY applicationname ");
            sqlCommand.Append(";");


            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }

        #endregion


        #region Private Methods

        private DbDataReader GetSchemaVersionFromGuid(Guid applicationId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_schemaversion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("applicationid = :applicationid ");
            sqlCommand.Append(";");
            
            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        private DbDataReader GetSchemaId(string applicationName)
        {
            return GetReader(
                readConnectionString,
                "mp_schemaversion",
                " WHERE applicationname ILIKE '" + applicationName.ToLower() + "'");

        }

        private DbDataReader GetReader(
            string connectionString,
            string tableName,
            string whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * ");
            sqlCommand.Append("from " + tableName.ToLower() + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append(" ; ");

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

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
        //    sqlCommand.Append("UPDATE " + tableName.ToLower() + " ");
        //    sqlCommand.Append(" SET " + dataFieldName.ToLower() + " = :fieldvalue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName.ToLower() + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append(" ; ");

        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    //NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        //    //connection.Open();
        //    //try
        //    //{
        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        connectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

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
        //    sqlCommand.Append("UPDATE " + tableName.ToLower() + " ");
        //    sqlCommand.Append(" SET " + dataFieldName.ToLower() + " = :fieldvalue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName.ToLower() + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append(" ; ");

        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = dataFieldValue;

        //    //NpgsqlConnection connection = new NpgsqlConnection(GetConnectionString());
        //    //connection.Open();
        //    //try
        //    //{
        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

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

        //public DataTable GetTable(
        //    string connectionString,
        //    string tableName,
        //    string whereClause)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("select * ");
        //    sqlCommand.Append("from " + tableName.ToLower() + " ");
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
        //        sqlCommand.Append("FROM	mp_sites ");
        //        sqlCommand.Append(";");

        //        count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //            readConnectionString,
        //            CommandType.Text,
        //            sqlCommand.ToString(),
        //            null));
        //    }
        //    catch (DbException) { }
        //    catch (InvalidOperationException) { }


        //    return count;
        //}
















        //public bool DeleteSchemaVersion(Guid applicationId)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_schemaversion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("applicationid = :applicationid ");
        //    sqlCommand.Append(";");

        //    //int rowsAffected = AdoHelper.ExecuteNonQuery(
        //    //    writeConnectionString,
        //    //    CommandType.StoredProcedure,
        //    //    "mp_schemaversion_delete(:applicationid)",
        //    //    arParams);

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}





        //public IDataReader SchemaVersionGetNonCore()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_schemaversion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("applicationid <> '077E4857-F583-488E-836E-34A4B04BE855' ");
        //    sqlCommand.Append("ORDER BY applicationname ");
        //    sqlCommand.Append(";");


        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.Text,
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

        //    NpgsqlParameter[] arParams = new NpgsqlParameter[6];

        //    arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new NpgsqlParameter("scriptfile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    arParams[2] = new NpgsqlParameter("runtime", NpgsqlTypes.NpgsqlDbType.Date);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = runTime;

        //    arParams[3] = new NpgsqlParameter("erroroccurred", NpgsqlTypes.NpgsqlDbType.Boolean);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = errorOccurred;

        //    arParams[4] = new NpgsqlParameter("errormessage", NpgsqlTypes.NpgsqlDbType.Text);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = errorMessage;

        //    arParams[5] = new NpgsqlParameter("scriptbody", NpgsqlTypes.NpgsqlDbType.Text);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = scriptBody;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_schemascripthistory (");
        //    sqlCommand.Append("applicationid, ");
        //    sqlCommand.Append("scriptfile, ");
        //    sqlCommand.Append("runtime, ");
        //    sqlCommand.Append("erroroccurred, ");
        //    sqlCommand.Append("errormessage, ");
        //    sqlCommand.Append("scriptbody )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":applicationid, ");
        //    sqlCommand.Append(":scriptfile, ");
        //    sqlCommand.Append(":runtime, ");
        //    sqlCommand.Append(":erroroccurred, ");
        //    sqlCommand.Append(":errormessage, ");
        //    sqlCommand.Append(":scriptbody )");
        //    sqlCommand.Append(";");
        //    sqlCommand.Append(" SELECT CURRVAL('mp_schemascripthistoryid_seq');");

        //    object obj = AdoHelper.ExecuteScalar(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //    int newID = Convert.ToInt32(obj);

        //    //int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //    //    writeConnectionString,
        //    //    CommandType.StoredProcedure,
        //    //    "mp_schemascripthistory_insert(:applicationid,:scriptfile,:runtime,:erroroccurred,:errormessage,:scriptbody)",
        //    //    arParams));

        //    return newID;


        //}

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_schemascripthistory_delete(:id)",
        //        arParams);

        //    return (rowsAffected > -1);

        //}

        //public IDataReader GetSchemaScriptHistory(int id)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_schemascripthistory_select_one(:id)",
        //        arParams);

        //}

        //public IDataReader GetSchemaScriptHistory(Guid applicationId)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_schemascripthistory_select_byapp(:applicationid)",
        //        arParams);

        //}

        //public IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_schemascripthistory_select_errorsbyapp(:applicationid)",
        //        arParams);

        //}

        //public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        //{

        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new NpgsqlParameter("scriptfile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    int count = 0;

        //    count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_schemascripthistory_exists(:applicationid,:scriptfile)",
        //        arParams));

        //    return (count > 0);

        //}





    }
}
