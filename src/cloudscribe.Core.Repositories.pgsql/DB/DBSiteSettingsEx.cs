// Author:					Joe Audette
// Created:				    2008-09-12
// Last Modified:			2015-01-16
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using cloudscribe.DbHelpers.pgsql;

namespace cloudscribe.Core.Repositories.pgsql
{
    
    internal static class DBSiteSettingsEx
    {
        
        public static IDataReader GetSiteSettingsExList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  e.* ");

            sqlCommand.Append("FROM	mp_sitesettingsex e ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_sitesettingsexdef d ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.keyname = d.keyname ");
            sqlCommand.Append("AND e.groupname = d.groupname ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.siteid = :siteid ");

            sqlCommand.Append("ORDER BY d.groupname, d.sortorder ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static async Task<bool> EnsureSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_sitesettingsex ");
            sqlCommand.Append("(");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("keyname, ");
            sqlCommand.Append("keyvalue, ");
            sqlCommand.Append("groupname ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("t.siteid, ");
            sqlCommand.Append("t.siteguid, ");
            sqlCommand.Append("t.keyname, ");
            sqlCommand.Append("t.defaultvalue, ");
            sqlCommand.Append("t.groupname  ");

            sqlCommand.Append("FROM ");

            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("d.keyname, ");
            sqlCommand.Append("d.defaultvalue, ");
            sqlCommand.Append("d.groupname ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_sites s, ");
            //sqlCommand.Append("FULL OUTER JOIN ");
            sqlCommand.Append("mp_sitesettingsexdef d ");
            sqlCommand.Append(") as t ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_sitesettingsex e ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.siteid = t.siteid ");
            sqlCommand.Append("AND e.keyname = t.keyname ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.siteid IS NULL ");
            sqlCommand.Append("; ");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return rowsAffected > 0;

        }

        public static bool SaveExpandoProperty(
           int siteId,
           Guid siteGuid,
           string groupName,
           string keyName,
           string keyValue)
        {
            int count = GetCount(siteId, keyName);
            if (count > 0)
            {
                return Update(siteId, keyName, keyValue);

            }
            else
            {
                return Create(siteId, siteGuid, keyName, keyValue, groupName);

            }

        }

        public static bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_sitesettingsex ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("keyvalue = :keyvalue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid <> :siteid AND ");
            sqlCommand.Append("keyname = :keyname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("keyname", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Value = keyName;

            arParams[2] = new NpgsqlParameter("keyvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        private static bool Create(
            int siteId,
            Guid siteGuid,
            string keyName,
            string keyValue,
            string groupName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_sitesettingsex (");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("keyname, ");
            sqlCommand.Append("keyvalue, ");
            sqlCommand.Append("groupname )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":keyname, ");
            sqlCommand.Append(":keyvalue, ");
            sqlCommand.Append(":groupname )");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("keyname", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[2].Value = keyName;

            arParams[3] = new NpgsqlParameter("keyvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Value = keyValue;

            arParams[4] = new NpgsqlParameter("groupname", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[4].Value = groupName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        private static bool Update(
            int siteID,
            string keyName,
            string keyValue)
        {
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_sitesettingsex ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("keyvalue = :keyvalue ");
			
			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("siteid = :siteid AND ");
			sqlCommand.Append("keyname = :keyname "); 
			sqlCommand.Append(";");
			
			NpgsqlParameter[] arParams = new NpgsqlParameter[3];
			
			arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer); 
			arParams[0].Value = siteID;
			
			arParams[1] = new NpgsqlParameter("keyname", NpgsqlTypes.NpgsqlDbType.Varchar,128); 
			arParams[1].Value = keyName;
			
			arParams[2] = new NpgsqlParameter("keyvalue", NpgsqlTypes.NpgsqlDbType.Text); 
			arParams[2].Value = keyValue;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
				CommandType.Text, 
				sqlCommand.ToString(), 
				arParams);
	
			return (rowsAffected > -1);
			
		}

        private static int GetCount(
            int siteID,
            string keyName)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitesettingsex ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid AND ");
            sqlCommand.Append("keyname = :keyname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteID;

            arParams[1] = new NpgsqlParameter("keyname", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
            arParams[1].Value = keyName;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


        }

        public static IDataReader GetDefaultExpandoSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitesettingsexdef ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);
        }

    }
}
