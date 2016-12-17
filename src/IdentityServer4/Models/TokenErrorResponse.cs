// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using System.Collections.Generic;

namespace IdentityServer4.Models
{
    public class TokenErrorResponse
    {
        public string Error { get; set; } = OidcConstants.TokenErrors.InvalidRequest;
        public string ErrorDescription { get; set; }

        public Dictionary<string, object> Custom { get; set; } = new Dictionary<string, object>();
    }
}