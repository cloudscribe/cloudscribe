// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results
{
    public class DiscoveryDocumentResult : IEndpointResult
    {
        public DiscoveryDocument Document { get; }
        public Dictionary<string, object> CustomEntries { get; }

        public DiscoveryDocumentResult(DiscoveryDocument document, Dictionary<string, object> customEntries)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            Document = document;
            CustomEntries = customEntries;
        }
        
        public Task ExecuteAsync(HttpContext context)
        {
            if (!CustomEntries.IsNullOrEmpty())
            {
                var jobject = ObjectSerializer.ToJObject(Document);
                jobject.AddDictionary(CustomEntries);

                return context.Response.WriteJsonAsync(jobject);
            }

            return context.Response.WriteJsonAsync(Document);
        }
    }
}