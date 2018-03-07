// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-03
// Last Modified:			2017-08-18
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

//https://github.com/aspnet/Antiforgery/blob/dev/src/Microsoft.AspNetCore.Antiforgery/Internal/DefaultAntiforgeryTokenStore.cs

namespace cloudscribe.Core.Identity
{
    public class SiteAntiforgeryTokenStore : IAntiforgeryTokenStore
    {
        private readonly AntiforgeryOptions _options;
        private MultiTenantOptions _multiTenantOptions;

        public SiteAntiforgeryTokenStore(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<AntiforgeryOptions> optionsAccessor
            )
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        private string GetCookieName(SiteContext tenant)
        {
            if(_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(!string.IsNullOrEmpty(tenant.SiteFolderName))
                {
                    return _options.Cookie.Name + tenant.SiteFolderName;
                }
            }

            return _options.Cookie.Name;
        }

        private string GetCookiePath(SiteContext tenant)
        {
            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!string.IsNullOrEmpty(tenant.SiteFolderName))
                {
                    return "/" + tenant.SiteFolderName;
                }
            }

            return "/";
        }

        public string GetCookieToken(HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            //var requestCookie = httpContext.Request.Cookies[_options.CookieName];

            var tenant = httpContext.GetTenant<SiteContext>();

            var requestCookie = httpContext.Request.Cookies[GetCookieName(tenant)];

            if (string.IsNullOrEmpty(requestCookie))
            {
                // unable to find the cookie.
                return null;
            }

            return requestCookie;
        }

        public async Task<AntiforgeryTokenSet> GetRequestTokensAsync(HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            //var cookieToken = httpContext.Request.Cookies[_options.CookieName];

            var tenant = httpContext.GetTenant<SiteContext>();
            var cookieToken = httpContext.Request.Cookies[GetCookieName(tenant)];

            StringValues requestToken;
            if (httpContext.Request.HasFormContentType)
            {
                // Check the content-type before accessing the form collection to make sure
                // we report errors gracefully.
                var form = await httpContext.Request.ReadFormAsync();
                requestToken = form[_options.FormFieldName];
            }

            // Fall back to header if the form value was not provided.
            if (requestToken.Count == 0 && _options.HeaderName != null)
            {
                requestToken = httpContext.Request.Headers[_options.HeaderName];
            }

            return new AntiforgeryTokenSet(requestToken, cookieToken, _options.FormFieldName, _options.HeaderName);
        }

        public void SaveCookieToken(HttpContext httpContext, string token)
        {
            Debug.Assert(httpContext != null);
            Debug.Assert(token != null);

            //var options = new CookieOptions() { HttpOnly = true };
            var options = _options.Cookie.Build(httpContext);


            //if (_options.RequireSsl)
            //{
            //    options.Secure = true;
            //}

            if (_options.Cookie.Path != null)
            {
                options.Path = _options.Cookie.Path.ToString();
            }
            else
            {
                var pathBase = httpContext.Request.PathBase.ToString();
                if (!string.IsNullOrEmpty(pathBase))
                {
                    options.Path = pathBase;
                }
            }


            var tenant = httpContext.GetTenant<SiteContext>();
            options.Path = new PathString(GetCookiePath(tenant));
            
            httpContext.Response.Cookies.Append(GetCookieName(tenant), token, options);
        }
    }

}
