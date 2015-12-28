// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:        Petr Kures
// Created:	      2007-09-05
// Last Modified: 2007-09-05
// This class encapsulates parameter handling for stored procedures and text commands.
// The use and distribution terms for this software are covered by the
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
// You must not remove this notice, or any other, from this software.
//
// 2007-11-23 modified by Joe Audette
// 2010-01-27 Joe Audette added parameter cache logic from Enterpise Data Block licensed under Ms-Pl http://www.codeplex.com/entlib
// Last Modified 2014-08-26 changed to use AdoHelper which allows us to profile with Glimpse ADO
// 2015-01-07 Joe Audette added async methods


using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
//using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
//using log4net;
using Microsoft.Extensions.Logging;


namespace cloudscribe.DbHelpers.MSSQL
{
    public class SqlParameterHelper
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SqlParameterHelper));
        //private static bool debugLog = log.IsDebugEnabled;
        private ILogger log = null;

        private SqlParameter[] arParams;
        private int index = 0;
        private int paramCnt;
        private string commandText;
        private bool parametersDefined;
        private string connectionString;
        private CommandType cmdType;


        #region Constructors

        public SqlParameterHelper(
            ILoggerFactory loggerFactory,
            string connectionString, 
            string commandText, 
            CommandType cmdType, 
            int paramCnt)
        {
            if(loggerFactory != null)
            {
                log = loggerFactory.CreateLogger(typeof(SqlParameterHelper).FullName);
            }
            
            //log = logger;
            Initialize(connectionString, commandText, cmdType, paramCnt);
        }

        public SqlParameterHelper(
            ILoggerFactory loggerFactory,
            string connectionString, 
            string commandText, 
            int paramCnt)
        {
            if (loggerFactory != null)
            {
                log = loggerFactory.CreateLogger(typeof(SqlParameterHelper).FullName);
            }
            
            //log = logger;
            Initialize(connectionString, commandText, CommandType.StoredProcedure, paramCnt);
        }

        #endregion

        private void BeginDefineSqlParameters()
        {
            index = 0;
            InitializeArray();
        }

        private void InitializeArray()
        {

# if dnx451
            if (
                (AppSettings.CacheMSSQLParameters)
                && (cmdType == CommandType.StoredProcedure)
                )
            {
                arParams = GetParameters(connectionString, commandText);
                Debug.Assert(arParams.Length == paramCnt, "parameters count doesn't match DB definition");
                parametersDefined = true;
            }
            else
            {
                arParams = new SqlParameter[paramCnt];
                parametersDefined = false;
            }
# else
            arParams = new SqlParameter[paramCnt];
            parametersDefined = false;
# endif
        }

        private void Initialize(string pConnectionInfo, string pCommandText, CommandType pCmdType, int pParamCnt)
        {
            if (pConnectionInfo == null || pConnectionInfo.Length == 0) throw new ArgumentNullException("connectionString");
            if (pCommandText == null || pCommandText.Length == 0) throw new ArgumentNullException("commandText");
            connectionString = pConnectionInfo;
            paramCnt = pParamCnt;
            commandText = pCommandText;
            cmdType = pCmdType;

            BeginDefineSqlParameters();
        }

        public void DefineSqlParameter(String paramName, SqlDbType type, ParameterDirection dir, object value)
        {
            DefineSqlParameter(paramName, type, string.Empty, 0, 0, dir, value, false, false);
        }

        public void DefineSqlParameter(String paramName, SqlDbType type, int size, ParameterDirection dir, object value)
        {
            DefineSqlParameter(paramName, type, string.Empty, size, 0, dir, value, true, false);
        }

        public void DefineSqlParameter(String paramName, SqlDbType type, int size, byte precision, ParameterDirection dir, object value)
        {
            DefineSqlParameter(paramName, type, string.Empty, size, precision, dir, value, true, true);
        }

        public void DefineSqlParameter(String paramName, SqlDbType type, String typeName, ParameterDirection dir, object value)
        {
            DefineSqlParameter(paramName, type, typeName, 0, 0, dir, value, false, false);
        }

        private void DefineSqlParameter(
            String paramName,
            SqlDbType type,
            String typeName,
            int size,
            byte precision,
            ParameterDirection dir,
            object value,
            bool sizeProvided,
            bool precisionProvided)
        {
            Debug.Assert(index < arParams.Length, "wrong number of parameters");
            Debug.Assert(index < paramCnt, "trying to define more parameters then defined count");

            if (!parametersDefined)
            {
                if (sizeProvided)
                    arParams[index] = new SqlParameter(paramName, type, size);
                else
                    arParams[index] = new SqlParameter(paramName, type);

                if (precisionProvided) { arParams[index].Precision = precision; }

                arParams[index].Direction = dir;
            }
            Debug.Assert(arParams[index].Direction == dir, "parameter's direction doesn't match cached parameters");
            Debug.Assert(string.Equals(arParams[index].ParameterName, paramName, StringComparison.CurrentCultureIgnoreCase), "parameter's name doesn't match cached parameters");
            Debug.Assert(
                ((type != SqlDbType.NText) && (arParams[index].SqlDbType == type))
                    ||
                ((type == SqlDbType.NText) && (arParams[index].SqlDbType == SqlDbType.NVarChar))
                ||
                ((type == SqlDbType.Image) && (arParams[index].SqlDbType == SqlDbType.VarBinary))
                , "parameter's type doesn't match cached parameters"
                );

            //commented out 2015-06-23 by JA
            // I think nowhere are we passing in typename
            // static AppSettings is going away, if we really need this later
            // we will have to pass in config object or pass in ownerprefix string (which would require significant re-work
            //if (typeName.Length > 0)
            //    arParams[index].TypeName = AppSettings.MSSQLOwnerPrefix + typeName;


            arParams[index].Value = value;
            index++;
        }

        public DbDataReader ExecuteReader()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            if (log != null) { log.LogDebug("ExecuteReader " + commandText); }
            
            return AdoHelper.ExecuteReader(connectionString, cmdType, commandText, arParams);
        }

        public DbDataReader ExecuteReader(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            if (log != null)  log.LogDebug("ExecuteReader " + commandText);
            return AdoHelper.ExecuteReader(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            if (log != null)  log.LogDebug("ExecuteReader " + commandText);
            return await AdoHelper.ExecuteReaderAsync(connectionString, cmdType, commandText, arParams, cancellationToken);
        }

        public async Task<DbDataReader> ExecuteReaderAsync(int commandTimeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            if (log != null)  log.LogDebug("ExecuteReader " + commandText);
            return await AdoHelper.ExecuteReaderAsync(connectionString, cmdType, commandText, commandTimeout, arParams, cancellationToken);
        }

        public int ExecuteNonQuery()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            if (log != null)  log.LogDebug("ExecuteReader " + commandText);
            return AdoHelper.ExecuteNonQuery(connectionString, cmdType, commandText, arParams);
        }

        public int ExecuteNonQuery(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            if (log != null) log.LogDebug("ExecuteReader " + commandText);
            return AdoHelper.ExecuteNonQuery(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            if (log != null) log.LogDebug("ExecuteReader " + commandText);
            return await AdoHelper.ExecuteNonQueryAsync(connectionString, cmdType, commandText, arParams, cancellationToken);
        }

        public async Task<int> ExecuteNonQueryAsync(int commandTimeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            if (log != null) log.LogDebug("ExecuteReader " + commandText);
            return await AdoHelper.ExecuteNonQueryAsync(connectionString, cmdType, commandText, commandTimeout, arParams, cancellationToken);
        }



        public object ExecuteScalar()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            if (log != null) log.LogDebug("ExecuteScalar " + commandText);
            return AdoHelper.ExecuteScalar(connectionString, cmdType, commandText, arParams);
        }

        public object ExecuteScalar(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            if (log != null) log.LogDebug("ExecuteScalar " + commandText);
            return AdoHelper.ExecuteScalar(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            if (log != null) log.LogDebug("ExecuteScalar " + commandText);
            return await AdoHelper.ExecuteScalarAsync(connectionString, cmdType, commandText, arParams, cancellationToken);
        }

        public async Task<object> ExecuteScalarAsync(int commandTimeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            //if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            if (log != null) log.LogDebug("ExecuteScalar " + commandText);
            return await AdoHelper.ExecuteScalarAsync(connectionString, cmdType, commandText, commandTimeout, arParams, cancellationToken);
        }


        

        public SqlParameter[] Parameters
        {
            get { return arParams; }
        }

        public string CommandText
        {
            get { return commandText; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        #region Caching of Params

        //===============================================================================
        // Microsoft patterns & practices Enterprise Library
        // Data Access Application Block
        //===============================================================================
        // Copyright © Microsoft Corporation.  All rights reserved.
        // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
        // OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
        // LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
        // FITNESS FOR A PARTICULAR PURPOSE.
        //===============================================================================
        // 
        // Forked From Enterprise Library licensed under Ms-Pl http://www.codeplex.com/entlib
        // but implementing a sub set of the API from the 2.0 Application Blocks SqlHelper
        // using implementation from the newer Ms-Pl version
        // Modifications by Joe Audette
        // Last Modified 2010-01-28

        

        private static SqlParameter[] CreateParameterCopy(DbCommand command)
        {
            DbParameterCollection parameters = command.Parameters;
            SqlParameter[] parameterArray = new SqlParameter[parameters.Count];
            parameters.CopyTo(parameterArray, 0);

            return SqlParameterCache.CloneParameters(parameterArray);
        }

        protected SqlParameter[] GetParametersFromCache(string connectionString, string procName)
        {
            return SqlParameterCache.GetCachedParameterSet(connectionString, procName);

        }

        private bool AlreadyCached(string connectionString, string procName)
        {
            return SqlParameterCache.IsParameterSetCached(connectionString, procName);
        }

        // the below is not supported in .net core
        // we could potentially get paramters via t-sql as in this thread
        //http://forums.asp.net/t/1108456.aspx?Is+their+any+alternative+way+instead+of+SqlCommandBuilder+DeriveParameters+

# if dnx451

        public SqlParameter[] GetParameters(string connectionString, string procName)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(procName)) throw new ArgumentNullException("Procedure Name");


            if (AlreadyCached(connectionString, procName))
            {
                return GetParametersFromCache(connectionString, procName);
            }
            else
            {
                SqlParameter[] copyOfParameters = DiscoverParameters(connectionString, procName);

                //this.cache.AddParameterSetToCache(connectionString, procName, copyOfParameters);
                SqlParameterCache.AddParameterSetToCache(connectionString, procName, copyOfParameters);

                return GetParametersFromCache(connectionString, procName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public SqlParameter[] DiscoverParameters(string connectionString, string procName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand discoveryCommand = new SqlCommand())
                {
                    discoveryCommand.CommandType = CommandType.StoredProcedure;
                    discoveryCommand.CommandText = procName;
                    discoveryCommand.Connection = connection;
                    connection.Open();
                    // SqlCommandBuilder is not available in .net core

                    SqlCommandBuilder.DeriveParameters(discoveryCommand);

                    

                    //connection.Close(); //not needed since inside using

                    // remove @ReturnVal
                    discoveryCommand.Parameters.RemoveAt(0);

                    SqlParameter[] result = new SqlParameter[discoveryCommand.Parameters.Count];
                    int i = 0;
                    foreach (SqlParameter parameter in discoveryCommand.Parameters)
                    {
                        //SqlParameter cloneParameter = (SqlParameter)((ICloneable)parameter).Clone();
                        SqlParameter cloneParameter = parameter.Copy();
                        
                        result[i] = cloneParameter;
                        i += 1;
                    }

                    return result;
                }
            }
        }

#endif


        #endregion

    }
}
