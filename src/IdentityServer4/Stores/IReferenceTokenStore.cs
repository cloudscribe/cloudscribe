// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Implements reference token storage
    /// </summary>
    public interface IReferenceTokenStore
    {
        Task StoreReferenceTokenAsync(string handle, Token token);
        Task<Token> GetReferenceTokenAsync(string handle);
        Task RemoveReferenceTokenAsync(string handle);
        Task RemoveReferenceTokensAsync(string subjectId, string clientId);
    }
}