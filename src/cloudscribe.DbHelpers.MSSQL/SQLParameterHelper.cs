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
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net;
using cloudscribe.Configuration;

namespace cloudscribe.DbHelpers.MSSQL
{
    public class SqlParameterHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlParameterHelper));
        private static bool debugLog = log.IsDebugEnabled;
      
        private SqlParameter[] arParams;
        private int index = 0;
        private int paramCnt;
        private string commandText;
        private bool parametersDefined;
        private string connectionString;
        private CommandType cmdType;

        #region Constructors

        public SqlParameterHelper(string connectionString, string commandText, CommandType cmdType, int paramCnt)
        {
            Initialize(connectionString, commandText, cmdType, paramCnt);
        }

        public SqlParameterHelper(string connectionString, string commandText, int paramCnt)
        {
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
            Debug.Assert(string.Equals(arParams[index].ParameterName, paramName, StringComparison.InvariantCultureIgnoreCase), "parameter's name doesn't match cached parameters");
            Debug.Assert(
                ((type != SqlDbType.NText) && (arParams[index].SqlDbType == type))
                    ||
                ((type == SqlDbType.NText) && (arParams[index].SqlDbType == SqlDbType.NVarChar))
                ||
                ((type == SqlDbType.Image) && (arParams[index].SqlDbType == SqlDbType.VarBinary))
                , "parameter's type doesn't match cached parameters"
                );


            if (typeName.Length > 0)
                arParams[index].TypeName = ConfigurationManager.AppSettings["MSSQLOwnerPrefix"] + typeName;


            arParams[index].Value = value;
            index++;
        }

        public IDataReader ExecuteReader()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            //return SqlHelper.ExecuteReader(connectionString, cmdType, commandText, arParams);
            return AdoHelper.ExecuteReader(connectionString, cmdType, commandText, arParams);
        }

        public IDataReader ExecuteReader(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            //return SqlHelper.ExecuteReader(connectionString, cmdType, commandText, commandTimeout, arParams);
            return AdoHelper.ExecuteReader(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<IDataReader> ExecuteReaderAsync()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            //return SqlHelper.ExecuteReader(connectionString, cmdType, commandText, arParams);
            return await AdoHelper.ExecuteReaderAsync(connectionString, cmdType, commandText, arParams);
        }

        public async Task<IDataReader> ExecuteReaderAsync(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteReader " + commandText); }
            //return SqlHelper.ExecuteReader(connectionString, cmdType, commandText, commandTimeout, arParams);
            return await AdoHelper.ExecuteReaderAsync(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public int ExecuteNonQuery()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            //return SqlHelper.ExecuteNonQuery(connectionString, cmdType, commandText, arParams);
            return AdoHelper.ExecuteNonQuery(connectionString, cmdType, commandText, arParams);
        }

        public int ExecuteNonQuery(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            //return SqlHelper.ExecuteNonQuery(connectionString, cmdType, commandText, commandTimeout, arParams);
            return AdoHelper.ExecuteNonQuery(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            //return SqlHelper.ExecuteNonQuery(connectionString, cmdType, commandText, arParams);
            return await AdoHelper.ExecuteNonQueryAsync(connectionString, cmdType, commandText, arParams);
        }

        public async Task<int> ExecuteNonQueryAsync(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteNonQuery " + commandText); }
            //return SqlHelper.ExecuteNonQuery(connectionString, cmdType, commandText, commandTimeout, arParams);
            return await AdoHelper.ExecuteNonQueryAsync(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        

        public object ExecuteScalar()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            //return SqlHelper.ExecuteScalar(connectionString, cmdType, commandText, arParams);
            return AdoHelper.ExecuteScalar(connectionString, cmdType, commandText, arParams);
        }

        public object ExecuteScalar(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            //return SqlHelper.ExecuteScalar(connectionString, cmdType, commandText, commandTimeout, arParams);
            return AdoHelper.ExecuteScalar(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        public async Task<object> ExecuteScalarAsync()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            //return SqlHelper.ExecuteScalar(connectionString, cmdType, commandText, arParams);
            return await AdoHelper.ExecuteScalarAsync(connectionString, cmdType, commandText, arParams);
        }

        public async Task<object> ExecuteScalarAsync(int commandTimeout)
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteScalar " + commandText); }
            //return SqlHelper.ExecuteScalar(connectionString, cmdType, commandText, commandTimeout, arParams);
            return await AdoHelper.ExecuteScalarAsync(connectionString, cmdType, commandText, commandTimeout, arParams);
        }

        //public object ExecuteScalar(SqlConnection conn)
        //{
        //    Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
        //    return SqlHelper.ExecuteScalar(conn, cmdType, commandText, arParams);
        //}

        public DataSet ExecuteDataset()
        {
            Debug.Assert((arParams.Length == index) && (paramCnt == index), "not all parameters were defined");
            if (debugLog) { log.Debug("ExecuteDataSet " + commandText); }
            //return SqlHelper.ExecuteDataset(connectionString, cmdType, commandText, arParams);
            return AdoHelper.ExecuteDataset(connectionString, cmdType, commandText, arParams);
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

        private static SqlParameter[] CreateParameterCopy(DbCommand command)
        {
            IDataParameterCollection parameters = command.Parameters;
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
                    SqlCommandBuilder.DeriveParameters(discoveryCommand);
                    //connection.Close(); //not needed since inside using

                    // remove @ReturnVal
                    discoveryCommand.Parameters.RemoveAt(0);

                    SqlParameter[] result = new SqlParameter[discoveryCommand.Parameters.Count];
                    int i = 0;
                    foreach (IDataParameter parameter in discoveryCommand.Parameters)
                    {
                        SqlParameter cloneParameter = (SqlParameter)((ICloneable)parameter).Clone();
                        result[i] = cloneParameter;
                        i += 1;
                    }

                    return result;
                }
            }
        }

        #endregion

    }
}
