// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using System.Net;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints.Results
{
    public class StatusCodeResult : IEndpointResult
    {
        public int StatusCode { get; }

        public StatusCodeResult(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        public StatusCodeResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCode;

            return Task.FromResult(0);
        }
    }
}