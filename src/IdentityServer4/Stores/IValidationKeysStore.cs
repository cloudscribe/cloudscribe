// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    public interface IValidationKeysStore
    {
        Task<IEnumerable<SecurityKey>> GetValidationKeysAsync();
    }
}