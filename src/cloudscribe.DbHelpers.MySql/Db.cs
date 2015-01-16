// Author:					Joe Audette
// Created:				    2004-08-03
// Last Modified:		    2014-09-19

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;


namespace cloudscribe.DbHelpers.MySql
{
    public class Db : IDb
    {
        public Db()
        { }

        private static readonly ILog log = LogManager.GetLogger(typeof(Db));

        public string DBPlatform
        {
            get { return "MySQL"; }
        }


        public DbException GetConnectionError(String overrideConnectionInfo)
        {
            DbException exception = null;

            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
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

        public bool CanAccessDatabase(String overrideConnectionInfo)
        {
            bool result = false;

            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
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
                  `FooID` int(11) NOT NULL auto_increment,
                  `Foo` varchar(255) NOT NULL default '',
                  PRIMARY KEY  (`FooID`)
                ) ENGINE=MyISAM  ;
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

        public bool CanAlterInnoDbSchema(string overrideConnectionInfo)
        {

            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(@"
                CREATE TABLE `mp_Testdb` (
                  `FooID` int(11) NOT NULL auto_increment,
                  `Foo` varchar(255) NOT NULL default '',
                  PRIMARY KEY  (`FooID`)
                ) ENGINE=InnoDB  ;
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
            sqlCommand.Append(" CREATE TEMPORARY TABLE IF NOT EXISTS Temptest ");
            sqlCommand.Append("(IndexID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,");
            sqlCommand.Append(" foo VARCHAR (100) NOT NULL);");
            sqlCommand.Append(" DROP TABLE Temptest;");
            try
            {
                RunScript(sqlCommand.ToString(), ConnectionString.GetWriteConnectionString());
            }
            catch (Exception)
            {
                result = false;
            }


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
            if ((script == null) || (script.Trim().Length == 0)) return true;

            bool result = false;
            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
            }

            connection.Open();

            MySqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // this fixed the problems with mysql 5.1
                MySqlScript mySqlScript = new MySqlScript(connection, script);
                mySqlScript.Execute();

                //this worked in all versions of mysql prior to 5.1
                //MySqlHelper.ExecuteNonQuery(
                //    connection,
                //    script, 
                //    null);

                transaction.Commit();
                result = true;

            }
            catch (MySqlException ex)
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
            sqlCommand.Append(" SET " + dataFieldName + " = ?fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?fieldValue", MySqlDbType.Blob);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append(" SET " + dataFieldName + " = ?fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?fieldValue", MySqlDbType.Blob);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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
            sqlCommand.Append(" ; ");

            return MySqlHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString());

        }

        public IDataReader GetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }

            return MySqlHelper.ExecuteReader(
                connectionString,
                query);

        }

        public int ExecteNonQuery(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                connectionString,
                query);

            return rowsAffected;

        }



        public static DataTable DatabaseHelperGetTable(
            String connectionString,
            String tableName,
            String whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append(" ; ");

            DataSet ds = MySqlHelper.ExecuteDataset(
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

                count = Convert.ToInt32(AdoHelper.ExecuteScalar(
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
            bool result = false;
            //return DatabaseHelper_TableExists("`mp_Sites`");
            try
            {
                using (IDataReader reader = GetSiteList())
                {
                    if (reader.Read())
                    {
                        //
                    }
                }
                // no error yet it must exist
                result = true;
            }
            catch { }

            return result;
        }

        private IDataReader GetSiteList()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Sites ");

            sqlCommand.Append("ORDER BY	SiteName ;");
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString());
        }

        public bool TableExists(string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetWriteConnectionString()))
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

            DataSet ds = MySqlHelper.ExecuteDataset(
                connectionString,
                sqlCommand.ToString());

            return ds.Tables[0];

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
                "mp_SchemaVersion",
                " WHERE LCASE(ApplicationName) = '" + applicationName.ToLower() + "'");

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
            sqlCommand.Append("?ApplicationID, ");
            sqlCommand.Append("?ApplicationName, ");
            sqlCommand.Append("?Major, ");
            sqlCommand.Append("?Minor, ");
            sqlCommand.Append("?Build, ");
            sqlCommand.Append("?Revision );");


            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ApplicationName = ?ApplicationName, ");
            sqlCommand.Append("Major = ?Major, ");
            sqlCommand.Append("Minor = ?Minor, ");
            sqlCommand.Append("Build = ?Build, ");
            sqlCommand.Append("Revision = ?Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ;");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        //public bool DeleteSchemaVersion(
        //    Guid applicationId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ApplicationID = ?ApplicationID ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();


        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);

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

        private IDataReader GetSchemaVersionFromGuid(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return MySqlHelper.ExecuteReader(
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

        //    return MySqlHelper.ExecuteReader(
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaScriptHistory (");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ScriptFile, ");
            sqlCommand.Append("RunTime, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage, ");
            sqlCommand.Append("ScriptBody )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ApplicationID, ");
            sqlCommand.Append("?ScriptFile, ");
            sqlCommand.Append("?RunTime, ");
            sqlCommand.Append("?ErrorOccurred, ");
            sqlCommand.Append("?ErrorMessage, ");
            sqlCommand.Append("?ScriptBody );");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new MySqlParameter("?RunTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new MySqlParameter("?ErrorOccurred", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intErrorOccurred;

            arParams[4] = new MySqlParameter("?ErrorMessage", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new MySqlParameter("?ScriptBody", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;


            int newID = 0;
            newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());
            return newID;

        }

        //public bool DeleteSchemaScriptHistory(int id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = ?ID ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;


        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
        //    sqlCommand.Append("ID = ?ID ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id;

        //    return MySqlHelper.ExecuteReader(
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
        //    sqlCommand.Append("ApplicationID = ?ApplicationID ");
        //    //sqlCommand.Append("AND ErrorOccurred = 0 ");

        //    sqlCommand.Append(" ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return MySqlHelper.ExecuteReader(
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
        //    sqlCommand.Append("ApplicationID = ?ApplicationID ");
        //    sqlCommand.Append("AND ErrorOccurred = 1 ");

        //    sqlCommand.Append(" ;");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    return MySqlHelper.ExecuteReader(
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
        //    sqlCommand.Append("ApplicationID = ?ApplicationID ");
        //    sqlCommand.Append("AND ScriptFile = ?ScriptFile ");

        //    sqlCommand.Append(" ;");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = applicationId.ToString();

        //    arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = scriptFile;

        //    int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}







    }
}
