// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    public class DefaultSigningCredentialsStore : ISigningCredentialStore
    {
        private readonly SigningCredentials _credential;

        public DefaultSigningCredentialsStore(SigningCredentials credential)
        {
            _credential = credential;
        }

        public Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_credential);
        }
    }
}