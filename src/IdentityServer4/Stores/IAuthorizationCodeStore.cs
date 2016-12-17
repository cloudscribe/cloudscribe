// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Implements storage for authorization codes
    /// </summary>
    public interface IAuthorizationCodeStore
    {
        Task StoreAuthorizationCodeAsync(string handle, AuthorizationCode code);
        Task<AuthorizationCode> GetAuthorizationCodeAsync(string code);
        Task RemoveAuthorizationCodeAsync(string code);
   }
}