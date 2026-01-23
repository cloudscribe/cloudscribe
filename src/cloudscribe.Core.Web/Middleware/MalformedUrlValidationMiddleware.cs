// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2026-01-23
// Last Modified:           2026-01-23
//

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    /// <summary>
    /// Middleware to detect and reject malformed returnUrl query parameters that contain
    /// recursive loops like returnUrl=/Account/Login?returnUrl=/Account/Login?returnUrl=...
    /// These can occur when crawlers follow navigation links repeatedly or when URLs are maliciously crafted.
    /// Returns 400 Bad Request for obviously broken returnUrl chains.
    /// </summary>
    public class MalformedUrlValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MalformedUrlValidationMiddleware> _logger;

        public MalformedUrlValidationMiddleware(
            RequestDelegate next,
            ILogger<MalformedUrlValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if there's a returnUrl query parameter (case-insensitive check)
            if (context.Request.Query.TryGetValue("returnUrl", out var returnUrlValues) ||
                context.Request.Query.TryGetValue("ReturnUrl", out returnUrlValues))
            {
                var returnUrl = returnUrlValues.ToString();

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // Decode the URL to check for recursive patterns
                    var decodedUrl = System.Net.WebUtility.UrlDecode(returnUrl);

                    // Check 1: Nested returnUrl= segments (indicates nesting)
                    // Count case-insensitive occurrences of "returnUrl=" in the decoded URL
                    var returnUrlCount2 = CountOccurrences(decodedUrl, "returnUrl=");
                    var returnUrlCountAlt = CountOccurrences(decodedUrl, "ReturnUrl=");
                    var returnUrlCountAlt2 = CountOccurrences(decodedUrl, "returnurl=");
                    var totalReturnUrlCount = returnUrlCount2 + returnUrlCountAlt + returnUrlCountAlt2;

                    if (totalReturnUrlCount > 0) // More than 0 means nested (returnUrl contains another returnUrl)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync("Bad Request");
                        return;
                    }

                    // Check 2: Multiple Login/Register patterns (indicates recursive loop)
                    // Count how many times /Account/Login or /Account/Register appears
                    var loginCount = CountOccurrences(decodedUrl, "/Account/Login");
                    var registerCount = CountOccurrences(decodedUrl, "/Account/Register");

                    // If either appears more than once, this is a recursive loop
                    if (loginCount > 1 || registerCount > 1)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync("Bad Request");
                        return;
                    }
                }
            }

            await _next(context);
        }

        /// <summary>
        /// Counts case-insensitive occurrences of a substring in a string
        /// </summary>
        private static int CountOccurrences(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
                return 0;

            int count = 0;
            int index = 0;

            while ((index = text.IndexOf(pattern, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += pattern.Length;
            }

            return count;
        }
    }
}
