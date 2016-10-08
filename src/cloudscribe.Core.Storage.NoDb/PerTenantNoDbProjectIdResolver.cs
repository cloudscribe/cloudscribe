// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-02
// Last Modified:			2016-10-08
// 


using cloudscribe.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{

    public class PerTenantNoDbProjectIdResolver
    {
        public PerTenantNoDbProjectIdResolver(
            SiteContext site)
        {
            this.site = site;
        }

        private SiteContext site;

        public Task<string> ResolveProjectId(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(site.Id.ToString());
        }

    }
}
