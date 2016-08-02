// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-02
// Last Modified:			2016-08-02
// 


using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    /// <summary>
    /// the goal is to be able to use a different NoDb projectid per site
    /// so that all site related data can be segragated on disk into its own proejct folder
    /// there are several considerations to address for this to work
    /// 
    /// the chicken or the egg problem, to lookup a site which is stored in NoDb we have to know the projectid where it is stored
    /// and since we want to determine the NoDb projectid based on the site we have a circular lookup problem, 
    /// we need to resolve the projectid in order to lookup the site in order to lookup the projectid
    /// 
    /// I thought about having a central list that maps host names folder etc but how to keep that list maintained is tricky
    /// perhaps a better solution is changing how we lookup sites both individually and as a list 
    /// so that we look for SiteSettings data in all project folders, it will require custom NoDb query logic for sites and site hosts
    /// but that would be more straight forward and less error prone than having some hidden list that we try to update from code
    /// if new sites are created from the UI
    /// 
    /// Note that ONLY SiteSettings would need to be looked up this way, once we have SiteSettings we have the projectid to use for all other data
    /// so this class can be simple, we just inject current SiteSettings and use Id.ToString() as the projectid
    /// 
    /// another consideration to be dealt with is CoreData ie country/state list info which is common across tenants
    /// to handle that we will make it always use DefaultProjectResolver so that data will always be stored in the default NoDb project
    /// 
    /// </summary>
    public class PerTenantNoDbProjectIdResolver
    {
        public PerTenantNoDbProjectIdResolver(
            SiteSettings site)
        {
            this.site = site;
        }

        private SiteSettings site;

        public Task<string> ResolveProjectId(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(site.Id.ToString());
        }

    }
}
