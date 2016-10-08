
// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-08-24
// Last Modified:		    2016-10-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;

namespace cloudscribe.Core.Web.Navigation
{
    /// <summary>
    /// this allows the navigation tree to be cached separate per site
    /// </summary>
    public class SiteNavigationCacheKeyResolver : ITreeCacheKeyResolver
    {
        public SiteNavigationCacheKeyResolver(SiteContext currentSite)
        {
            this.currentSite = currentSite;
        }

        private SiteContext currentSite;

        public string GetCacheKey(INavigationTreeBuilder builder)
        {
            return builder.Name + currentSite.Id.ToString();
        }
    }
}
