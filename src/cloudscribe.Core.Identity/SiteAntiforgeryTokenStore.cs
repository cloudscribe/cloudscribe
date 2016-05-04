// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-03
// Last Modified:			2016-05-04
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteAntiforgeryTokenStore : IAntiforgeryTokenStore
    {
        private readonly AntiforgeryOptions _options;
        private readonly IAntiforgeryTokenSerializer _tokenSerializer;

        public SiteAntiforgeryTokenStore(
            IOptions<AntiforgeryOptions> optionsAccessor,
            IAntiforgeryTokenSerializer tokenSerializer)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            if (tokenSerializer == null)
            {
                throw new ArgumentNullException(nameof(tokenSerializer));
            }

            _options = optionsAccessor.Value;
            _tokenSerializer = tokenSerializer;
        }

        public AntiforgeryToken GetCookieToken(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var services = httpContext.RequestServices;
            var contextAccessor = services.GetRequiredService<IAntiforgeryContextAccessor>();
            if (contextAccessor.Value != null)
            {
                return contextAccessor.Value.CookieToken;
            }

            var tenant = httpContext.GetTenant<SiteSettings>();

            var requestCookie = httpContext.Request.Cookies[_options.CookieName + tenant.SiteFolderName];
            if (string.IsNullOrEmpty(requestCookie))
            {
                // unable to find the cookie.
                return null;
            }

            return _tokenSerializer.Deserialize(requestCookie);
        }

        public async Task<AntiforgeryTokenSet> GetRequestTokensAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var tenant = httpContext.GetTenant<SiteSettings>();
            var requestCookie = httpContext.Request.Cookies[_options.CookieName + tenant.SiteFolderName];
            if (string.IsNullOrEmpty(requestCookie))
            {
                throw new InvalidOperationException(
                    "Resources.FormatAntiforgery_CookieToken_MustBeProvided(_options.CookieName)");
            }

            if (!httpContext.Request.HasFormContentType)
            {
                // Check the content-type before accessing the form collection to make sure
                // we throw gracefully.
                throw new InvalidOperationException(
                    "Resources.FormatAntiforgery_FormToken_MustBeProvided(_options.FormFieldName)");
            }

            var form = await httpContext.Request.ReadFormAsync();
            var formField = form[_options.FormFieldName];
            if (string.IsNullOrEmpty(formField))
            {
                throw new InvalidOperationException(
                    "Resources.FormatAntiforgery_FormToken_MustBeProvided(_options.FormFieldName)");
            }

            return new AntiforgeryTokenSet(formField, requestCookie);
        }

        public void SaveCookieToken(HttpContext httpContext, AntiforgeryToken token)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Add the cookie to the request based context.
            // This is useful if the cookie needs to be reloaded in the context of the same request.

            var services = httpContext.RequestServices;
            var contextAccessor = services.GetRequiredService<IAntiforgeryContextAccessor>();
            Debug.Assert(contextAccessor.Value == null, "AntiforgeryContext should be set only once per request.");
            contextAccessor.Value = new AntiforgeryContext() { CookieToken = token };

            var serializedToken = _tokenSerializer.Serialize(token);
            var options = new CookieOptions() { HttpOnly = true };


            // Note: don't use "newCookie.Secure = _options.RequireSSL;" since the default
            // value of newCookie.Secure is poulated out of band.
            if (_options.RequireSsl)
            {
                options.Secure = true;
            }
            var tenant = httpContext.GetTenant<SiteSettings>();
            if (tenant.SiteFolderName.Length > 0) options.Path = new PathString("/" + tenant.SiteFolderName);
            httpContext.Response.Cookies.Append(_options.CookieName + tenant.SiteFolderName, serializedToken, options);
        }
    }

}

// code below pasted from rc2 code here
// https://github.com/aspnet/Antiforgery/blob/dev/src/Microsoft.AspNetCore.Antiforgery/Internal/DefaultAntiforgeryTokenStore.cs
// but rc1 is very different so may have to punt on this for now

//using System;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Http;
//using Microsoft.Extensions.OptionsModel;
//using Microsoft.Extensions.Primitives;
//using Microsoft.AspNet.Antiforgery;

//namespace cloudscribe.Core.Web.Components
//{
//    public class SiteAntiforgeryTokenStore : IAntiforgeryTokenStore
//    {
//        private readonly AntiforgeryOptions _options;

//        public SiteAntiforgeryTokenStore(IOptions<AntiforgeryOptions> optionsAccessor)
//        {
//            if (optionsAccessor == null)
//            {
//                throw new ArgumentNullException(nameof(optionsAccessor));
//            }

//            _options = optionsAccessor.Value;
//        }

//        public string GetCookieToken(HttpContext httpContext)
//        {
//            Debug.Assert(httpContext != null);

//            var requestCookie = httpContext.Request.Cookies[_options.CookieName];
//            if (string.IsNullOrEmpty(requestCookie))
//            {
//                // unable to find the cookie.
//                return null;
//            }

//            return requestCookie;
//        }

//        public async Task<AntiforgeryTokenSet> GetRequestTokensAsync(HttpContext httpContext)
//        {
//            Debug.Assert(httpContext != null);

//            var cookieToken = httpContext.Request.Cookies[_options.CookieName];

//            StringValues requestToken;
//            if (httpContext.Request.HasFormContentType)
//            {
//                // Check the content-type before accessing the form collection to make sure
//                // we report errors gracefully.
//                var form = await httpContext.Request.ReadFormAsync();
//                requestToken = form[_options.FormFieldName];
//            }

//            // Fall back to header if the form value was not provided.
//            if (requestToken.Count == 0 && _options.HeaderName != null)
//            {
//                requestToken = httpContext.Request.Headers[_options.HeaderName];
//            }

//            return new AntiforgeryTokenSet(requestToken, cookieToken, _options.FormFieldName, _options.HeaderName);
//        }

//        public void SaveCookieToken(HttpContext httpContext, string token)
//        {
//            Debug.Assert(httpContext != null);
//            Debug.Assert(token != null);

//            var options = new CookieOptions() { HttpOnly = true };

//            // Note: don't use "newCookie.Secure = _options.RequireSSL;" since the default
//            // value of newCookie.Secure is poulated out of band.
//            if (_options.RequireSsl)
//            {
//                options.Secure = true;
//            }

//            httpContext.Response.Cookies.Append(_options.CookieName, token, options);
//        }
//    }
//}
