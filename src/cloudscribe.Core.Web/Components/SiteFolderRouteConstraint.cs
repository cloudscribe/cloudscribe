// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-19
// Last Modified:			2016-03-03
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        
        // TODO: is this getting called on requests for static resources?
        // want to make sure it is not, we don't want to hit the db on requests for static files
        // though tenants should be cached

        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string parameterName,
            IDictionary<string,object> values,
            RouteDirection routeDirection)
        {
            
            string requestFolder = httpContext.Request.Path.StartingSegment();
            
            var tenant = httpContext.GetTenant<SiteSettings>();

            if (tenant != null)
            {
                if(tenant.SiteFolderName == requestFolder) { return true; }
               
            }

            return false;
        }
    }
}
