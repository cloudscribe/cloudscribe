// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-08-24
// Last Modified:		    2019-09-28
// 

using cloudscribe.Core.Models;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using System.Threading.Tasks;

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

        public Task<string> GetCacheKey(INavigationTreeBuilder builder)
        {
            var result=  builder.Name + currentSite.Id.ToString();
            return Task.FromResult(result);
        }
    }
}
