// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Hosting;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results
{
    public class JsonWebKeysResult : IEndpointResult
    {
        public IEnumerable<JsonWebKey> WebKeys { get; }

        public JsonWebKeysResult(IEnumerable<JsonWebKey> webKeys)
        {
            WebKeys = webKeys;
        }
        
        public Task ExecuteAsync(HttpContext context)
        {
            return context.Response.WriteJsonAsync(new { keys = WebKeys });
        }
    }
}