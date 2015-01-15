// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-15
// 
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using Npgsql;
using cloudscribe.DbHelpers.pgsql;

namespace cloudscribe.Core.Repositories.pgsql
{
    
    internal static class DBSiteFolder
    {
        
        public static int Add(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_insert(:guid,:siteguid,:foldername)",
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
           
            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = folderName;

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_update(:guid,:siteguid,:foldername)",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(
            Guid guid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_delete(:guid)",
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(Guid guid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_select_one(:guid)",
                arParams);

        }

        public static IDataReader GetBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_select_bysite(:siteguid)",
                arParams);

        }

        public static bool Exists(string folderName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[0].Value = folderName;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_exists(:foldername)",
                arParams));

            return (count > 0);

        }


        public static Guid GetSiteGuid(string folderName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = folderName;

            string strGuid = AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitefolders_selectsiteguid(:foldername)",
                arParams).ToString();

            return new Guid(strGuid);

        }

        public static IDataReader GetAll()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("sf.guid, ");
            sqlCommand.Append("sf.foldername ");

            sqlCommand.Append("FROM	mp_sitefolders sf ");

            sqlCommand.Append("JOIN	mp_sites s ");

            sqlCommand.Append("ON sf.siteguid = s.siteguid ");

            sqlCommand.Append("ORDER BY sf.foldername ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);
        }

        public static int GetCount()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitefolders ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));
        }

        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }



            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("s.siteid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("sf.guid, ");
            sqlCommand.Append("sf.foldername ");

            sqlCommand.Append("FROM	mp_sitefolders sf  ");
            sqlCommand.Append("JOIN	mp_sites s ");

            sqlCommand.Append("ON sf.siteguid = s.siteguid ");

            sqlCommand.Append("ORDER BY sf.foldername ");

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }
	

    }
}
