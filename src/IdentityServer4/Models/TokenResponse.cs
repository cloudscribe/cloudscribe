// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Models
{
    public class TokenResponse
    {
        public string IdentityToken { get; set; }
        public string AccessToken { get; set; }
        public int AccessTokenLifetime { get; set; }
        public string RefreshToken { get; set; }

        public Dictionary<string, object> Custom { get; set; } = new Dictionary<string, object>();
    }
}