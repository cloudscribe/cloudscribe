// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-28
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{
    internal class DBSiteUser
    {
        internal DBSiteUser(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public DbDataReader GetUserList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_Select", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_GetCountByMonthYear", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        //public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_Users_SmartDropDown", 
        //        3);

        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@Query", SqlDbType.NVarChar, 50, ParameterDirection.Input, query);
        //    sph.DefineSqlParameter("@RowsToGet", SqlDbType.Int, ParameterDirection.Input, rowsToGet);
        //    return sph.ExecuteReader();
        //}

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_EmailLookup", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Query", SqlDbType.NVarChar, 50, ParameterDirection.Input, query);
            sph.DefineSqlParameter("@RowsToGet", SqlDbType.Int, ParameterDirection.Input, rowsToGet);
            return sph.ExecuteReader();
        }

        public int UserCount(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_Count", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public async Task<int> CountUsers(
            int siteId, 
            string userNameBeginsWith,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountByFirstLetter", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 1, ParameterDirection.Input, userNameBeginsWith);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountByRegistrationDateRange", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountOnlineSinceTime", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectUsersOnlineSinceTime", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            return sph.ExecuteReader();
        }

        public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectTop50UsersOnlineSinceTime", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            return sph.ExecuteReader();
        }


        public async Task<int> GetNewestUserId(int siteId, CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_GetNewestUserID", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken
            )
        {
           
            SqlParameterHelper sph;

            switch (sortMode)
            {
                case 1:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectPageByDateDesc", 
                        4);

                    break;

                case 2:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectPageSortLF", 
                        4);

                    break;

                case 0:
                default:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectPage", 
                        4);

                    break;
            }
            
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 50, ParameterDirection.Input, userNameBeginsWith);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<int> CountUsersForSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountForSearch", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            
            SqlParameterHelper sph;

            switch (sortMode)
            {
                case 1:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectSearchPageByDateDesc", 
                        4);

                    break;

                case 2:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectSearchPageByLF", 
                        4);

                    break;

                case 0:
                default:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectSearchPage", 
                        4);

                    break;
            }
            
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<int> CountUsersForAdminSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountForAdminSearch", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            
            SqlParameterHelper sph;

            switch (sortMode)
            {
                case 1:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectAdminSearchPageByDateDesc", 
                        4);

                    break;

                case 2:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectAdminSearchPageByLF", 
                        4);

                    break;

                case 0:
                default:
                    sph = new SqlParameterHelper(
                        logFactory,
                        readConnectionString, 
                        "mp_Users_SelectAdminSearchPage", 
                        4);

                    break;
            }



            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<int> CountLockedOutUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountLocked", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectLockedPage", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<int> CountEmailUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_CountEmailUnconfirmed",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetPageEmailUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_UnconfirmedEmailPage",
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<int> CountPhoneUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_CountPhoneUnconfirmed",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetPagePhoneUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_UnconfirmedPhonePage",
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }


        public async Task<int> CountFutureLockoutDate(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_CountFutureLockoutDate",
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@CurrentUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);

            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetFutureLockoutPage(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Users_FutureLockoutPage",
                4);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@CurrentUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }



        public async Task<int> CountNotApprovedUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_CountNotApproved", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            int count = Convert.ToInt32(result);
            return count;
        }

        public async Task<DbDataReader> GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectNotApprovedPage", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        //public static DataTable GetUserListPageTable(int siteId, int pageNumber, int pageSize, string userNameBeginsWith)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_Users_SelectPage", 4);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 1, ParameterDirection.Input, userNameBeginsWith);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserID", typeof(int));
        //    dataTable.Columns.Add("Name", typeof(String));
        //    dataTable.Columns.Add("DateCreated", typeof(DateTime));
        //    dataTable.Columns.Add("WebSiteUrl", typeof(String));
        //    dataTable.Columns.Add("TotalPosts", typeof(int));

        //    using (IDataReader reader = sph.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dataTable.NewRow();
        //            row["UserID"] = Convert.ToInt32(reader["UserID"]);
        //            row["Name"] = reader["Name"].ToString();
        //            row["DateCreated"] = Convert.ToDateTime(reader["DateCreated"]);
        //            row["WebSiteUrl"] = reader["WebSiteUrl"].ToString();
        //            row["TotalPosts"] = Convert.ToInt32(reader["TotalPosts"]);
        //            dataTable.Rows.Add(row);

        //        }

        //    }

        //    return dataTable;

        //}

        public async Task<int> AddUser(
            Guid siteGuid,
            int siteId,
            string fullName,
            string loginName,
            string email,
            Guid userGuid,
            DateTime dateCreated,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            DateTime? dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc, 
            bool accountApproved,
            bool isLockedOut,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string authorBio,
            string comment,

            string normalizedUserName,
            string loweredEmail,
            bool canAutoLockout,
            
            CancellationToken cancellationToken
            )
        {

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_Insert", 
                32);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@DateCreated", SqlDbType.DateTime, ParameterDirection.Input, dateCreated);
            sph.DefineSqlParameter("@MustChangePwd", SqlDbType.Bit, ParameterDirection.Input, mustChangePwd);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 32, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@EmailChangeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, Guid.Empty);

            if (!dateOfBirth.HasValue)
            {
                sph.DefineSqlParameter("@DateOfBirth", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@DateOfBirth", SqlDbType.DateTime, ParameterDirection.Input, dateOfBirth);
            }

            sph.DefineSqlParameter("@EmailConfirmed", SqlDbType.Bit, ParameterDirection.Input, emailConfirmed);
            sph.DefineSqlParameter("@PasswordHash", SqlDbType.NVarChar, -1, ParameterDirection.Input, passwordHash);
            sph.DefineSqlParameter("@SecurityStamp", SqlDbType.NVarChar, -1, ParameterDirection.Input, securityStamp);
            sph.DefineSqlParameter("@PhoneNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, phoneNumber);
            sph.DefineSqlParameter("@PhoneNumberConfirmed", SqlDbType.Bit, ParameterDirection.Input, phoneNumberConfirmed);
            sph.DefineSqlParameter("@TwoFactorEnabled", SqlDbType.Bit, ParameterDirection.Input, twoFactorEnabled);

            if (!lockoutEndDateUtc.HasValue)
            {
                sph.DefineSqlParameter("@LockoutEndDateUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@LockoutEndDateUtc", SqlDbType.DateTime, ParameterDirection.Input, lockoutEndDateUtc);
            }

            sph.DefineSqlParameter("@AccountApproved", SqlDbType.Bit, ParameterDirection.Input, accountApproved);
            sph.DefineSqlParameter("@IsLockedOut", SqlDbType.Bit, ParameterDirection.Input, isLockedOut);
            sph.DefineSqlParameter("@DisplayInMemberList", SqlDbType.Bit, ParameterDirection.Input, displayInMemberList);
            sph.DefineSqlParameter("@WebSiteURL", SqlDbType.NVarChar, 100, ParameterDirection.Input, webSiteUrl);
            sph.DefineSqlParameter("@Country", SqlDbType.NVarChar, 100, ParameterDirection.Input, country);
            sph.DefineSqlParameter("@State", SqlDbType.NVarChar, 100, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@AvatarUrl", SqlDbType.NVarChar, 250, ParameterDirection.Input, avatarUrl);
            sph.DefineSqlParameter("@Signature", SqlDbType.NVarChar, -1, ParameterDirection.Input, signature);
            sph.DefineSqlParameter("@AuthorBio", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorBio);
            sph.DefineSqlParameter("@Comment", SqlDbType.NVarChar, -1, ParameterDirection.Input, comment);

            sph.DefineSqlParameter("@NormalizedUserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, normalizedUserName);
            sph.DefineSqlParameter("@LoweredEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, loweredEmail);
            sph.DefineSqlParameter("@CanAutoLockout", SqlDbType.Bit, ParameterDirection.Input, canAutoLockout);

            

            object result = await sph.ExecuteScalarAsync(cancellationToken);

            int newID = Convert.ToInt32(result);
            return newID;
        }

        public async Task<bool> UpdateUser(
            int userId,
            string name,
            string loginName,
            string email,
            string gender,
            bool accountApproved,
            bool trusted,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string loweredEmail,
            string comment,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId, 
            string newEmail,
            bool rolesChanged,
            string authorBio,
            DateTime? dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,
            bool isLockedOut,

            string normalizedUserName,
            bool newEmailApproved,
            bool canAutoLockout,
            DateTime? lastPasswordChangedDate,

            CancellationToken cancellationToken
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_Update", 
                35);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Gender", SqlDbType.NChar, 1, ParameterDirection.Input, gender);
            sph.DefineSqlParameter("@AccountApproved", SqlDbType.Bit, ParameterDirection.Input, accountApproved);
            sph.DefineSqlParameter("@Trusted", SqlDbType.Bit, ParameterDirection.Input, trusted);
            sph.DefineSqlParameter("@DisplayInMemberList", SqlDbType.Bit, ParameterDirection.Input, displayInMemberList);
            sph.DefineSqlParameter("@WebSiteUrl", SqlDbType.NVarChar, 100, ParameterDirection.Input, webSiteUrl);
            sph.DefineSqlParameter("@Country", SqlDbType.NVarChar, 100, ParameterDirection.Input, country);
            sph.DefineSqlParameter("@State", SqlDbType.NVarChar, 100, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@AvatarUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, avatarUrl);
            sph.DefineSqlParameter("@Signature", SqlDbType.NVarChar, -1, ParameterDirection.Input, signature);
            sph.DefineSqlParameter("@LoweredEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, loweredEmail);
            sph.DefineSqlParameter("@Comment", SqlDbType.NVarChar, -1, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@MustChangePwd", SqlDbType.Bit, ParameterDirection.Input, mustChangePwd);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 32, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@NewEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, newEmail);
            sph.DefineSqlParameter("@RolesChanged", SqlDbType.Bit, ParameterDirection.Input, rolesChanged);
            sph.DefineSqlParameter("@AuthorBio", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorBio);

            if (!dateOfBirth.HasValue)
            {
                sph.DefineSqlParameter("@DateOfBirth", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@DateOfBirth", SqlDbType.DateTime, ParameterDirection.Input, dateOfBirth);
            }

            sph.DefineSqlParameter("@EmailConfirmed", SqlDbType.Bit, ParameterDirection.Input, emailConfirmed);
            sph.DefineSqlParameter("@PasswordHash", SqlDbType.NVarChar, -1, ParameterDirection.Input, passwordHash);
            sph.DefineSqlParameter("@SecurityStamp", SqlDbType.NVarChar, -1, ParameterDirection.Input, securityStamp);
            sph.DefineSqlParameter("@PhoneNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, phoneNumber);
            sph.DefineSqlParameter("@PhoneNumberConfirmed", SqlDbType.Bit, ParameterDirection.Input, phoneNumberConfirmed);
            sph.DefineSqlParameter("@TwoFactorEnabled", SqlDbType.Bit, ParameterDirection.Input, twoFactorEnabled);
            if (!lockoutEndDateUtc.HasValue)
            {
                sph.DefineSqlParameter("@LockoutEndDateUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@LockoutEndDateUtc", SqlDbType.DateTime, ParameterDirection.Input, lockoutEndDateUtc);
            }

            sph.DefineSqlParameter("@IsLockedOut", SqlDbType.Bit, ParameterDirection.Input, isLockedOut);

            sph.DefineSqlParameter("@NormalizedUserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, normalizedUserName);
            sph.DefineSqlParameter("@NewEmailApproved", SqlDbType.Bit, ParameterDirection.Input, newEmailApproved);
            sph.DefineSqlParameter("@CanAutoLockout", SqlDbType.Bit, ParameterDirection.Input, canAutoLockout);

            if (!lastPasswordChangedDate.HasValue)
            {
                sph.DefineSqlParameter("@LastPasswordChangedDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@LastPasswordChangedDate", SqlDbType.DateTime, ParameterDirection.Input, lastPasswordChangedDate);
            }


            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }


        public async Task<bool> DeleteUser(
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_Delete", 
                1);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> DeleteUsersBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_Users_DeleteBySite",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_UpdateLastActivityTime", 
                2);

            sph.DefineSqlParameter("@UserID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LastUpdate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid, 
            DateTime lastLoginTime,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_UpdateLastLoginTime", 
                2);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LastLoginTime", SqlDbType.DateTime, ParameterDirection.Input, lastLoginTime);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        //public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_UpdateLastPasswordChangeTime", 
        //        2);

        //    sph.DefineSqlParameter("@UserID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@PasswordChangeTime", SqlDbType.DateTime, ParameterDirection.Input, lastPasswordChangeTime);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public bool UpdateFailedPasswordAttemptStartWindow(
        //    Guid userGuid,
        //    DateTime windowStartTime)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_SetFailedPasswordAttemptStartWindow", 
        //        2);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@WindowStartTime", SqlDbType.DateTime, ParameterDirection.Input, windowStartTime);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_SetFailedPasswordAttemptCount", 
                2);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@AttemptCount", SqlDbType.Int, ParameterDirection.Input, attemptCount);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        //public bool UpdateFailedPasswordAnswerAttemptStartWindow(
        //    Guid userGuid,
        //    DateTime windowStartTime)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_SetFailedPasswordAnswerAttemptStartWindow", 
        //        2);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@WindowStartTime", SqlDbType.DateTime, ParameterDirection.Input, windowStartTime);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public bool UpdateFailedPasswordAnswerAttemptCount(
        //    Guid userGuid,
        //    int attemptCount)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_SetFailedPasswordAnswerAttemptCount", 
        //        2);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@AttemptCount", SqlDbType.Int, ParameterDirection.Input, attemptCount);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public async Task<bool> AccountLockout(
            Guid userGuid, 
            DateTime lockoutTime,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_AccountLockout", 
                2);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LockoutTime", SqlDbType.DateTime, ParameterDirection.Input, lockoutTime);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> AccountClearLockout(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_AccountClearLockout", 
                1);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        //public async Task<bool> SetRegistrationConfirmationGuid(
        //    Guid userGuid, 
        //    Guid registrationConfirmationGuid,
        //    CancellationToken cancellationToken)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_SetRegistrationConfirmationGuid", 
        //        2);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registrationConfirmationGuid);
        //    int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
        //    return (rowsAffected > -1);
        //}

        //public async Task<bool> ConfirmRegistration(
        //    Guid emptyGuid, 
        //    Guid registrationConfirmationGuid,
        //    CancellationToken cancellationToken)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_ConfirmRegistration", 
        //        2);

        //    sph.DefineSqlParameter("@EmptyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, emptyGuid);
        //    sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registrationConfirmationGuid);
        //    int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
        //    return (rowsAffected > -1);
        //}

        //public bool UpdatePasswordAndSalt(
        //    int userId,
        //    int pwdFormat,
        //    string password,
        //    string passwordSalt)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_UpdatePasswordAndSalt", 
        //        4);

        //    sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
        //    sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
        //    sph.DefineSqlParameter("@PasswordSalt", SqlDbType.NVarChar, 128, ParameterDirection.Input, passwordSalt);
        //    sph.DefineSqlParameter("@PwdFormat", SqlDbType.Int, ParameterDirection.Input, pwdFormat);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public bool UpdatePasswordQuestionAndAnswer(
        //    Guid userGuid,
        //    string passwordQuestion,
        //    string passwordAnswer)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_UpdatePasswordQuestionAndAnswer", 
        //        3);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    sph.DefineSqlParameter("@PasswordQuestion", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordQuestion);
        //    sph.DefineSqlParameter("@PasswordAnswer", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordAnswer);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public async Task<bool> UpdateTotalRevenue(Guid userGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_UpdateTotalRevenueByUser", 
        //        1);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
        //    int rowsAffected = await sph.ExecuteNonQueryAsync();
        //    return rowsAffected > 0;

        //}

        //public async Task<bool> UpdateTotalRevenue()
        //{
        //    int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
        //        writeConnectionString,
        //        CommandType.StoredProcedure,
        //        "mp_Users_UpdateTotalRevenue",
        //        null);

        //    return rowsAffected > 0;
        //}


        public async Task<bool> FlagAsDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_FlagAsDeleted", 
                1);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<bool> FlagAsNotDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Users_FlagAsNotDeleted", 
                1);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        //public bool IncrementTotalPosts(int userId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_IncrementTotalPosts", 
        //        1);

        //    sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        //public bool DecrementTotalPosts(int userId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_Users_DecrementTotalPosts", 
        //        1);

        //    sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public async Task<DbDataReader> GetRolesByUser(
            int siteId, 
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_GetUserRoles", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }


        public async Task<DbDataReader> GetUserByRegistrationGuid(
            int siteId, 
            Guid registerConfirmGuid,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectByRegisterGuid", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registerConfirmGuid);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }


        public async Task<DbDataReader> GetSingleUser(
            int siteId, 
            string email,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectByEmail", 
                2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectAllByEmail", 
                1);

            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetSingleUserByLoginName(
            int siteId, 
            string loginName, 
            bool allowEmailFallback,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectByLoginName", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@AllowEmailFallback", SqlDbType.Bit, ParameterDirection.Input, allowEmailFallback);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public DbDataReader GetSingleUserByLoginNameNonAsync(int siteId, string loginName, bool allowEmailFallback)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectByLoginName", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@AllowEmailFallback", SqlDbType.Bit, ParameterDirection.Input, allowEmailFallback);
            return sph.ExecuteReader();
        }

        public async Task<DbDataReader> GetSingleUser(
            int userId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectOne", 
                1);

            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetSingleUser(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_SelectByGuid", 
                1);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        //public Guid GetUserGuidFromOpenId(
        //    int siteId,
        //    string openIduri)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_Users_SelectGuidByOpenIDURI", 
        //        2);

        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@OpenIDURI", SqlDbType.NVarChar, 100, ParameterDirection.Input, openIduri);

        //    Guid userGuid = Guid.Empty;

        //    using (DbDataReader reader = sph.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {
        //            userGuid = new Guid(reader["UserGuid"].ToString());
        //        }
        //    }

        //    return userGuid;
        //}

        //public Guid GetUserGuidFromWindowsLiveId(
        //    int siteId,
        //    string windowsLiveId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_Users_SelectGuidByWindowsLiveID", 
        //        2);

        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@WindowsLiveID", SqlDbType.NVarChar, 100, ParameterDirection.Input, windowsLiveId);

        //    Guid userGuid = Guid.Empty;

        //    using (DbDataReader reader = sph.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {
        //            userGuid = new Guid(reader["UserGuid"].ToString());
        //        }
        //    }

        //    return userGuid;
        //}

        public string LoginByEmail(int siteId, string email, string password)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_LoginByEmail", 
                4);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.InputOutput, null);
            sph.ExecuteNonQuery();


            if (sph.Parameters[3] != null)
            {
                return sph.Parameters[3].Value.ToString();
            }
            else
            {
                return string.Empty;
            }

        }

        public string Login(int siteId, string loginName, string password)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Users_Login", 
                4);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.InputOutput, null);
            sph.ExecuteNonQuery();


            if (sph.Parameters[3] != null)
            {
                return sph.Parameters[3].Value.ToString();
            }
            else
            {
                return string.Empty;
            }

        }



        //public DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_UserProperties_SelectByUser", 
        //        1);

        //    sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserGuid", typeof(String));
        //    dataTable.Columns.Add("PropertyName", typeof(String));
        //    dataTable.Columns.Add("PropertyValueString", typeof(String));
        //    // dataTable.Columns.Add("PropertyValueBinary", typeof(object));

        //    using (DbDataReader reader = sph.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dataTable.NewRow();
        //            row["UserGuid"] = reader["UserGuid"].ToString();
        //            row["PropertyName"] = reader["PropertyName"].ToString();
        //            row["PropertyValueString"] = reader["PropertyValueString"].ToString();
        //            //row["PropertyValueBinary"] = reader["PropertyValueBinary"];
        //            dataTable.Rows.Add(row);
        //        }

        //    }

        //    return dataTable;
        //}

        public DbDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserProperties_SelectOne", 
                2);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            return sph.ExecuteReader();
        }

        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_UserProperties_PropertyExists", 
                2);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserProperties_Insert", 
                7);

            sph.DefineSqlParameter("@PropertyID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, propertyId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            sph.DefineSqlParameter("@PropertyValueString", SqlDbType.NVarChar, -1, ParameterDirection.Input, propertyValue);
            sph.DefineSqlParameter("@PropertyValueBinary", SqlDbType.VarBinary, -1, ParameterDirection.Input, propertyValueBinary);
            sph.DefineSqlParameter("@LastUpdatedDate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateDate);
            sph.DefineSqlParameter("@isLazyLoaded", SqlDbType.Bit, ParameterDirection.Input, isLazyLoaded);
            sph.ExecuteNonQuery();
        }

        public void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserProperties_Update", 
                6);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            sph.DefineSqlParameter("@PropertyValueString", SqlDbType.NVarChar, -1, ParameterDirection.Input, propertyValue);
            sph.DefineSqlParameter("@PropertyValueBinary", SqlDbType.VarBinary, -1, ParameterDirection.Input, propertyValueBinary);
            sph.DefineSqlParameter("@LastUpdatedDate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateDate);
            sph.DefineSqlParameter("@isLazyLoaded", SqlDbType.Bit, ParameterDirection.Input, isLazyLoaded);
            sph.ExecuteNonQuery();
        }

        public bool DeletePropertiesByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_UserProperties_DeleteByUser", 
                1);

            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }





    }
}
