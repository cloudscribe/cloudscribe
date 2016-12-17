// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteJsonAsync(this HttpResponse response, object o)
        {
            var json = ObjectSerializer.ToString(o);
            await response.WriteJsonAsync(json);
        }

        public static async Task WriteJsonAsync(this HttpResponse response, string json)
        {
            response.ContentType = "application/json";
            await response.WriteAsync(json);
        }

        public static void SetNoCache(this HttpResponse response)
        {
            if (!response.Headers.ContainsKey("Cache-Control"))
            {
                response.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");
            }
            if (!response.Headers.ContainsKey("Pragma"))
            {
                response.Headers.Add("Pragma", "no-cache");
            }
        }

        public static async Task WriteHtmlAsync(this HttpResponse response, string html)
        {
            response.ContentType = "text/html; charset=UTF-8";
            await response.WriteAsync(html, Encoding.UTF8);
        }

        public static void RedirectToAbsoluteUrl(this HttpResponse response, string url)
        {
            if (url.IsLocalUrl())
            {
                if (url.StartsWith("~/")) url = url.Substring(1);
                url = response.HttpContext.GetIdentityServerBaseUrl().EnsureTrailingSlash() + url.RemoveLeadingSlash();
            }
            response.Redirect(url);
        }
    }
}