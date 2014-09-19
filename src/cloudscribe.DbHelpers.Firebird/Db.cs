// Author:					Joe Audette
// Created:				    2007-07-17
// Last Modified:		    2014-09-19

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;

namespace cloudscribe.DbHelpers.Firebird
{
    public class Db : IDb
    {
        public Db()
        {

        }

        private static readonly ILog log = LogManager.GetLogger(typeof(Db));

        public string DBPlatform
        {
            get { return "FirebirdSql"; }
        }

        private string GetConnectionString()
        {
            return AppSettings.FirebirdConnectionString;

        }

        public DbException GetConnectionError(string overrideConnectionInfo)
        {
            DbException exception = null;

            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
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
            // TODO: FXCop says we should not swallow unspecific exceptions

            bool result = false;

            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
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
                CREATE TABLE MP_TESTDB (
                  FOOID INTEGER NOT NULL ,
                  FOO VARCHAR(255) NOT NULL ,
                  PRIMARY KEY (FOOID)
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



            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE MP_TESTDB ADD MOREFOO varchar(255) ;");

            try
            {
                RunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE MP_TESTDB;");

            try
            {
                RunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }

            return result;
        }

        public bool CanCreateTemporaryTables()
        {
            // TODO: no temp tables supported, but currently not needed
            return true;
        }

        public bool RunScript(
            FileInfo scriptFile,
            String overrideConnectionInfo)
        {
            if (scriptFile == null) return false;

            if (
                (overrideConnectionInfo == null)
                || (overrideConnectionInfo.Length == 0)
              )
            {
                overrideConnectionInfo = GetConnectionString();
            }

            if (scriptFile.FullName.EndsWith(".config"))
            {
                string pathToScripts = scriptFile.FullName.Replace(".config", string.Empty);
                if (Directory.Exists(pathToScripts))
                {
                    DirectoryInfo scriptDirectory
                        = new DirectoryInfo(pathToScripts);

                    FileInfo[] scriptFiles
                        = scriptDirectory.GetFiles("*.config");

                    Array.Sort(scriptFiles, CompareFileNames);

                    foreach (FileInfo file in scriptFiles)
                    {

                        bool result = AdoHelper.ExecuteBatchScript(
                            overrideConnectionInfo,
                            file.FullName);

                        if (!result)
                        {
                            log.Error("Failed with no exception running script "
                            + file.FullName);
                        }

                    }
                }

                return true;
            }

            return false;

        }

        private static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }



        public bool RunScript(
            string script,
            string overrideConnectionInfo)
        {
            if ((script == null) || (script.Length == 0)) return true;

            bool result = false;
            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
            }

            connection.Open();

            FbTransaction transaction = connection.BeginTransaction();

            try
            {
                AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, script, null);

                transaction.Commit();
                result = true;

            }
            catch (FbException ex)
            {
                transaction.Rollback();
                log.Error("dbPortal.RunScript failed", ex);
                throw;
            }
            finally
            {
                connection.Close();

            }

            return result;
        }

        private bool RunScriptByStatements(
            DirectoryInfo scriptDirectory,
            string overrideConnectionInfo)
        {
            if (scriptDirectory == null) return false;

            bool result = false;


            FileInfo[] scriptFiles = scriptDirectory.GetFiles("*.config");

            Array.Sort(scriptFiles, CompareFileNames);

            foreach (FileInfo scriptFile in scriptFiles)
            {
                if (
                (overrideConnectionInfo == null)
                || (overrideConnectionInfo.Length == 0)
              )
                {
                    overrideConnectionInfo = GetConnectionString();
                }

                result = AdoHelper.ExecuteBatchScript(
                    overrideConnectionInfo,
                    scriptFile.FullName);
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@fieldValue", FbDbType.VarChar);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);

        }

        public bool UpdateTableField(
            string tableName,
            string keyFieldName,
            string keyFieldValue,
            string dataFieldName,
            string dataFieldValue,
            string additionalWhere)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@fieldValue", FbDbType.VarChar);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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
            sqlCommand.Append(" ; ");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString());

        }

        public IDataReader GetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

            return AdoHelper.ExecuteReader(
               connectionString,
               query);
        }

        public int ExecteNonQuery(
           string connectionString,
           string query
           )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               connectionString,
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
            sqlCommand.Append(" ; ");

            DataSet ds = AdoHelper.ExecuteDataset(
                connectionString,
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

                count =  Convert.ToInt32(AdoHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    sqlCommand.ToString(),
                    null));

            }
            catch (DbException) { }
            catch (InvalidOperationException) { }
            
            return count;

        }

        public bool SitesTableExists()
        {
            return TableExists("MP_SITES");
        }

        public bool TableExists(string tableName)
        {
            FbConnection connection = new FbConnection(GetConnectionString());
            string[] restrictions = new string[4];
            restrictions[2] = tableName;
            connection.Open();
            DataTable table = connection.GetSchema("Tables", restrictions);
            connection.Close();
            if (table != null)
            {
                return (table.Rows.Count > 0);
            }

            return false;
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
                ConnectionString.GetReadConnectionString(),
                "MP_SCHEMAVERSION",
                " WHERE UPPER(ApplicationName) = '" + applicationName.ToUpper() + "'");

        }

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
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ApplicationName, ");
            sqlCommand.Append("@Major, ");
            sqlCommand.Append("@Minor, ");
            sqlCommand.Append("@Build, ");
            sqlCommand.Append("@Revision );");


            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter("@ApplicationName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new FbParameter("@Major", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new FbParameter("@Minor", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new FbParameter("@Build", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new FbParameter("@Revision", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ApplicationID = @ApplicationID ;");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter("@ApplicationName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new FbParameter("@Major", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new FbParameter("@Minor", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new FbParameter("@Build", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new FbParameter("@Revision", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;


            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }


        //public bool DeleteSchemaVersion(
        //    Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), arParams);

        //    return (rowsAffected > 0);

        //}


        private IDataReader GetSchemaVersionFromGuid(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
        //        ConnectionString.GetReadConnectionString(),
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

            #region Bit Conversion
            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter(":ApplicationID", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter(":ScriptFile", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new FbParameter(":RunTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new FbParameter(":ErrorOccurred", FbDbType.SmallInt);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intErrorOccurred;

            arParams[4] = new FbParameter(":ErrorMessage", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new FbParameter(":ScriptBody", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SCHEMASCRIPTHISTORY_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = @ID ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@ID", FbDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    int rowsAffected = AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
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
        //    sqlCommand.Append("ID = @ID ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@ID", FbDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
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
        //    //sqlCommand.Append("AND ErrorOccurred = 0 ");

        //    sqlCommand.Append(" ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
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
        //    sqlCommand.Append("AND ErrorOccurred = 1 ");

        //    sqlCommand.Append(" ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) ");
        //    sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = @ApplicationID ");
        //    sqlCommand.Append("AND ScriptFile = @ScriptFile ");

        //    sqlCommand.Append(" ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new FbParameter("@ScriptFile", FbDbType.VarChar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}



    }
}
