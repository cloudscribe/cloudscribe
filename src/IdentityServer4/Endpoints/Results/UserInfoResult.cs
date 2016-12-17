// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints.Results
{
    internal class UserInfoResult : IEndpointResult
    {
        public Dictionary<string, object> Claims;

        public UserInfoResult(Dictionary<string, object> claims)
        {
            Claims = claims;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.SetNoCache();
            await context.Response.WriteJsonAsync(Claims);
        }
    }
}