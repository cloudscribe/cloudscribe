// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Default reference token store.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.DefaultGrantStore{IdentityServer4.Models.Token}" />
    /// <seealso cref="IdentityServer4.Stores.IReferenceTokenStore" />
    public class DefaultReferenceTokenStore : DefaultGrantStore<Token>, IReferenceTokenStore
    {
        public DefaultReferenceTokenStore(
            IPersistedGrantStore store, 
            PersistentGrantSerializer serializer, 
            ILogger<DefaultReferenceTokenStore> logger) 
            : base(Constants.PersistedGrantTypes.ReferenceToken, store, serializer, logger)
        {
        }

        /// <summary>
        /// Stores the reference token asynchronous.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task StoreReferenceTokenAsync(string handle, Token token)
        {
            return StoreItemAsync(handle, token, token.ClientId, token.SubjectId, token.CreationTime, token.Lifetime);
        }

        /// <summary>
        /// Gets the reference token asynchronous.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        public Task<Token> GetReferenceTokenAsync(string handle)
        {
            return GetItemAsync(handle);
        }

        /// <summary>
        /// Removes the reference token asynchronous.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        public Task RemoveReferenceTokenAsync(string handle)
        {
            return RemoveItemAsync(handle);
        }

        /// <summary>
        /// Removes the reference tokens asynchronous.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public Task RemoveReferenceTokensAsync(string subjectId, string clientId)
        {
            return RemoveAllAsync(subjectId, clientId);
        }
    }
}