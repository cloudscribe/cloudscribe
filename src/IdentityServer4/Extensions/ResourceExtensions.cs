// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IdentityServer4.Models
{
    internal static class ResourceExtensions
    {
        public static IEnumerable<string> ToScopeNames(this Resources resources)
        {
            var scopes = from api in resources.ApiResources
                         where api.Scopes != null
                         from scope in api.Scopes
                         select scope.Name;
            if (resources.OfflineAccess)
            {
                scopes = scopes.Union(new string[] { IdentityServerConstants.StandardScopes.OfflineAccess });
            }
            return resources.IdentityResources.Select(x => x.Name).Union(scopes).ToArray();
        }

        public static ApiResource FindApiResourceByScope(this Resources resources, string name)
        {
            var q = from api in resources.ApiResources
                    where api.Scopes != null
                    from scope in api.Scopes
                    where scope.Name == name
                    select api;
            return q.FirstOrDefault();
        }

        public static Scope FindApiScope(this Resources resources, string name)
        {
            var q = from api in resources.ApiResources
                    where api.Scopes != null
                    from scope in api.Scopes
                    where scope.Name == name
                    select scope;
            return q.FirstOrDefault();
        }

        public static Scope FindApiScope(this ApiResource api, string name)
        {
            if (api == null || api.Scopes == null) return null;

            var q = from s in api.Scopes
                    where s.Name == name
                    select s;
            return q.FirstOrDefault();
        }

        internal static Resources FilterEnabled(this Resources resources)
        {
            if (resources == null) return new Resources();

            var identity = resources.IdentityResources.Where(x => x.Enabled);

            var api = from a in resources.ApiResources
                      where a.Enabled
                      select a;

            return new Resources(identity, api)
            {
                OfflineAccess = resources.OfflineAccess
            };
        }
    }
}