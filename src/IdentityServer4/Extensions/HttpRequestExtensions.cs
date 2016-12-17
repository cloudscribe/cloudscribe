// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static string GetCorsOrigin(this HttpRequest request)
        {
            var origin = request.Headers["Origin"].FirstOrDefault();
            var thisOrigin = request.Scheme + "://" + request.Host;

            // see if the Origin is different than this server's origin. if so
            // that indicates a proper CORS request. some browsers send Origin
            // on POST requests.
            if (origin != null && origin != thisOrigin)
            {
                return origin;
            }

            return null;
        }
    }
}