using cloudscribe.Configuration;
using log4net;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace cloudscribe.DbHelpers.MSSQL
{
    public class SetupHelper
    {
        private Db db;

        public SetupHelper()
        {
            db = new Db();

        }

        public IDataReader GetApplicationId(string applicationName)
        {
            return db.GetReader(
                ConnectionString.GetReadConnectionString(),
                "mp_SchemaVersion",
                " WHERE LOWER(ApplicationName) = '" + applicationName.ToLower() + "'");
        }

        /// <summary>
        /// Schemas the version_ add schema version.
        /// </summary>
        /// <param name="applicationID">The application Guid.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="major">major</param>
        /// <param name="minor">minor</param>
        /// <param name="build">build</param>
        /// <param name="revision">revision</param>
        /// <returns></returns>
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

        public bool DeleteSchemaVersion(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Delete", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public bool SchemaVersionExists(Guid applicationId)
        {
            bool result = false;

            using (IDataReader reader = GetSchemaVersion(applicationId))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }



        public IDataReader GetSchemaVersion(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectOne", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        public IDataReader SchemaVersionGetNonCore()
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectNonCore", 0);
            return sph.ExecuteReader();
        }

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

        public bool DeleteSchemaScriptHistory(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaScriptHistory_Delete", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public IDataReader GetSchemaScriptHistory(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectOne", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            return sph.ExecuteReader();
        }

        public IDataReader GetSchemaScriptHistory(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectByApp", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        public IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectErrorsByApp", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        public bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_Exists", 2);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }
    }
}
