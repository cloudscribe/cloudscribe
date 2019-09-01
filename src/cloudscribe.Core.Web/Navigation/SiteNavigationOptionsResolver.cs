// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette/Derek Gray
// Created:				    2016-08-24
// Last Modified:		    2019-09-01
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
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.environment = environment;
        }

        private IWebHostEnvironment environment;
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
