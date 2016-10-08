// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-08-24
// Last Modified:		    2016-10-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace cloudscribe.Core.Web.Navigation
{
    /// <summary>
    /// this allows pluggin in a different navigation.xml file per tenant if needed
    /// for example to add custom admin menus in a specific site
    /// </summary>
    public class SiteNavigationOptionsResolver : IOptions<NavigationOptions>
    {
        public SiteNavigationOptionsResolver(
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.environment = environment;
        }

        private IHostingEnvironment environment;
        private IHttpContextAccessor httpContextAccessor;

        public NavigationOptions Value
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var currentSite = context.GetTenant<SiteContext>();
                var options = new NavigationOptions();
                var siteXmlFileName = "navigation." + currentSite.Id.ToString() + ".xml";
                var siteXmlNavigationFilePath = Path.Combine(environment.ContentRootPath, siteXmlFileName);
                if(File.Exists(siteXmlNavigationFilePath))
                {
                    options.NavigationMapXmlFileName = siteXmlFileName;
                }
                // TODO: do we care to support json?

                return options;
            }
        }

    }
}
