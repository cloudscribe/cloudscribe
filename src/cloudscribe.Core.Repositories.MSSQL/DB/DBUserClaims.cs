// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-22
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using cloudscribe.DbHelpers.MSSQL;


namespace cloudscribe.Core.Repositories.MSSQL
{

    internal static class DBUserClaims
    {

        public static int Create(
            string userId,
            string claimType,
            string claimValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_Insert", 3);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        
        
        public static bool Delete(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_Delete", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId, string claimType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_DeleteByUserByType", 2);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(string userId, string claimType, string claimValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_DeleteExactByUser", 3);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@ClaimType", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimType);
            sph.DefineSqlParameter("@ClaimValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, claimValue);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserClaims_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

       
        public static IDataReader GetByUser(string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserClaims_SelectByUser", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            return sph.ExecuteReader();

        }

        

    }

}
