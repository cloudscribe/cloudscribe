// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-19
// Last Modified:			2015-11-18
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Components
{
    /// <summary>
    /// used when creating routes for folder based multi tenant scenarios
    /// constrains the route based on the first url segment which identifies
    /// the site
    /// </summary>
    public class SiteFolderRouteConstraint : IRouteConstraint
    {
        public SiteFolderRouteConstraint()
        {
            
        }

        //private string requiredSiteFolder;

        //public SiteFolderRouteConstraint(string folderParam)
        //{
        //    requiredSiteFolder = folderParam;
        //}

        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string parameterName,
            IDictionary<string,object> values,
            RouteDirection routeDirection)
        {
            string requestFolder = RequestSiteResolver.GetFirstFolderSegment(httpContext.Request.Path);
            //return string.Equals(requiredSiteFolder, requestFolder, StringComparison.CurrentCultureIgnoreCase);
            ISiteResolver siteResolver = httpContext.ApplicationServices.GetService<ISiteResolver>();
            if(siteResolver != null)
            {
                try
                {
                    // exceptions expected here until db install scripts have run or if db connection error
                    ISiteSettings site = siteResolver.Resolve();
                    if ((site != null) && (site.SiteFolderName == requestFolder)) { return true; }
                }
                catch
                {
                    // do we need to log this?
                }

            }

            return false;
        }
    }
}
