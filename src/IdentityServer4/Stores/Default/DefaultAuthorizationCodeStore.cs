// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Default authorization code store.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.DefaultGrantStore{IdentityServer4.Models.AuthorizationCode}" />
    /// <seealso cref="IdentityServer4.Stores.IAuthorizationCodeStore" />
    public class DefaultAuthorizationCodeStore : DefaultGrantStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public DefaultAuthorizationCodeStore(
            IPersistedGrantStore store, 
            PersistentGrantSerializer serializer, 
            ILogger<DefaultAuthorizationCodeStore> logger) 
            : base(Constants.PersistedGrantTypes.AuthorizationCode, store, serializer, logger)
        {
        }

        /// <summary>
        /// Stores the authorization code asynchronous.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Task StoreAuthorizationCodeAsync(string handle, AuthorizationCode code)
        {
            return StoreItemAsync(handle, code, code.ClientId, code.Subject.GetSubjectId(), code.CreationTime, code.Lifetime);
        }

        /// <summary>
        /// Gets the authorization code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Task<AuthorizationCode> GetAuthorizationCodeAsync(string code)
        {
            return GetItemAsync(code);
        }

        /// <summary>
        /// Removes the authorization code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Task RemoveAuthorizationCodeAsync(string code)
        {
            return RemoveItemAsync(code);
        }
    }
}