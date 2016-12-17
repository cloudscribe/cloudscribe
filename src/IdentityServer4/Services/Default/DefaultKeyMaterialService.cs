// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.Stores;

namespace IdentityServer4.Services.Default
{
    public class DefaultKeyMaterialService : IKeyMaterialService
    {
        private readonly ISigningCredentialStore _signingCredential;
        private readonly IEnumerable<IValidationKeysStore> _validationKeys;

        public DefaultKeyMaterialService(IEnumerable<IValidationKeysStore> validationKeys, ISigningCredentialStore signingCredential = null)
        {
            _signingCredential = signingCredential;
            _validationKeys = validationKeys;
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            if (_signingCredential != null)
            {
                return await _signingCredential.GetSigningCredentialsAsync();
            }

            return null;
        }

        public async Task<IEnumerable<SecurityKey>> GetValidationKeysAsync()
        {
            var keys = new List<SecurityKey>();

            foreach (var store in _validationKeys)
            {
                keys.AddRange(await store.GetValidationKeysAsync());
            }

            return keys;
        }
    }
}