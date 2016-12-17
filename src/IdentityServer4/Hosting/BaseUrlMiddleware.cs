// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting
{
    public class BaseUrlMiddleware
    {
        private readonly RequestDelegate _next;
        
        public BaseUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            var origin = request.Scheme + "://" + request.Host.Value;
            context.SetOrigin(origin);
            context.SetBasePath(request.PathBase.Value.RemoveTrailingSlash());

            await _next(context);
        }
    }
}