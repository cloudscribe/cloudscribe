// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-19
// Last Modified:			2016-02-04
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

        //private string requiredSiteFolder;

        //public SiteFolderRouteConstraint(string folderParam)
        //{
        //    requiredSiteFolder = folderParam;
        //}

        //TODO: is this getting called on requests for static resources?
        // want to make sure it is not, we don't want to hit the db on requests for static files

        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string parameterName,
            IDictionary<string,object> values,
            RouteDirection routeDirection)
        {
            // TODO: problem here is we want to make this method be async
            //https://github.com/aspnet/Mvc/issues/1094
            // but may have to jump through some hoops
            // for now we are calling an async method synchronously while blocking the thread

            string requestFolder = httpContext.Request.Path.StartingSegment();
            //return string.Equals(requiredSiteFolder, requestFolder, StringComparison.CurrentCultureIgnoreCase);
            ITenantResolver<SiteSettings> siteResolver 
                = httpContext.ApplicationServices.GetService<ITenantResolver<SiteSettings>>();

            if(siteResolver != null)
            {
                try
                {
                    // exceptions expected here until db install scripts have run or if db connection error
                    //ISiteSettings site = siteResolver.Resolve();
                    //var siteContext = Task.Run<TenantContext<SiteSettings>>(fun => siteResolver.ResolveAsync(httpContext));
                    //

                    Func<Task<TenantContext<SiteSettings>>> f  = delegate ()
                    {
                        return siteResolver.ResolveAsync(httpContext);
                    };

                    //http://stackoverflow.com/questions/22628087/calling-async-method-synchronously
                    var siteContext = Task.Run<TenantContext<SiteSettings>>(f).Result;

                    if ((siteContext != null) 
                        &&(siteContext.Tenant != null) 
                        && (siteContext.Tenant.SiteFolderName == requestFolder)) { return true; }
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
