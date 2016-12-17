// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results
{
    public class RevocationErrorResult : IEndpointResult
    {
        public string Error { get; set; }

        public RevocationErrorResult(string error)
        {
            Error = error;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteJsonAsync(new { error = Error });
        }
    }
}
