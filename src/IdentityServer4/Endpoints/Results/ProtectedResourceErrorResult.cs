// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.Extensions.Primitives;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints.Results
{
    internal class ProtectedResourceErrorResult : IEndpointResult
    {
        public string Error;
        public string ErrorDescription;

        public ProtectedResourceErrorResult(string error, string errorDescription = null)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.SetNoCache();

            if (Constants.ProtectedResourceErrorStatusCodes.ContainsKey(Error))
            {
                context.Response.StatusCode = Constants.ProtectedResourceErrorStatusCodes[Error];
            }

            var parameter = string.Format("error=\"{0}\"", Error);
            if (ErrorDescription.IsPresent())
            {
                parameter = string.Format("{0}, error_description=\"{1}\"",
                    parameter, ErrorDescription);
            }

            context.Response.Headers.Add("WwwAuthentication", new StringValues("Bearer"));

            return Task.FromResult(0);
        }
    }
}