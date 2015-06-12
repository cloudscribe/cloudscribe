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
// renamed from internal class CachingMechanism
// License Ms-Pl http://www.codeplex.com/entlib
// with modifications by Joe Audette
// Last Modified: 2010-01-27

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

//namespace Microsoft.Practices.EnterpriseLibrary.Data
namespace cloudscribe.DbHelpers.MSSQL
{
    /// <devdoc>
    /// CachingMechanism provides caching support for stored procedure 
    /// parameter discovery and caching
    /// </devdoc>
    //internal class SqlParameterCache 
    public sealed class SqlParameterCache
    {
        private SqlParameterCache() { }

        //private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        //private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private static ConcurrentDictionary<string, SqlParameter[]> paramCache 
            = new ConcurrentDictionary<string, SqlParameter[]>();

        /// <devdoc>
        /// Create and return a copy of the IDataParameter array.
        /// </devdoc>        
        public static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                // ICloneable not supported in .net core
                //clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
                clonedParameters[i] = ((SqlParameter)originalParameters[i]).Copy();
            }

            return clonedParameters;
        }

        

        ///// <devdoc>
        ///// Empties all items from the cache
        ///// </devdoc>
        //public void Clear()
        //{
        //    this.paramCache.Clear();
        //}

        /// <devdoc>
        /// Add a parameter array to the cache for the command.
        /// </devdoc>        
        public static void AddParameterSetToCache(string connectionString, string storedProcedure, SqlParameter[] parameters)
        {
            string key = CreateHashKey(connectionString, storedProcedure);
            paramCache[key] = parameters;
        }

        /// <devdoc>
        /// Gets a parameter array from the cache for the command. Returns null if no parameters are found.
        /// </devdoc>        
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string storedProcedure)
        {
            string key = CreateHashKey(connectionString, storedProcedure);
            SqlParameter[] cachedParameters = (SqlParameter[])(paramCache[key]);
            return CloneParameters(cachedParameters);
        }

        /// <devdoc>
        /// Gets if a given stored procedure on a specific connection string has a cached parameter set
        /// </devdoc>        
        public static bool IsParameterSetCached(string connectionString, string storedProcedure)
        {
            string hashKey = CreateHashKey(connectionString, storedProcedure);
            return paramCache[hashKey] != null;
        }

        private static string CreateHashKey(string connectionString, string storedProcedure)
        {
            return connectionString + ":" + storedProcedure;
        }
    }
}
