// Author:					Joe Audette
// Created:				    2014-09-06
// Last Modified:		    2014-09-07
// 
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using cloudscribe.Core.Web;

namespace cloudscribe.Web.Routing
{
    /// <summary>
    /// used when creating routes for folder based multi tenant scenarios
    /// constrains the route based on the first url segment which identifies
    /// the site
    /// </summary>
    public class SiteFolderRouteConstraint : IRouteConstraint
    {
        private string requiredSiteFolder;

        public SiteFolderRouteConstraint(string folderParam)
        {
            requiredSiteFolder = folderParam;
        }

        public bool Match(
            HttpContextBase httpContext, 
            Route route, 
            string parameterName, 
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            string requestFolder = SiteContext.GetFirstFolderSegment(httpContext.Request.RawUrl);
            return string.Equals(requiredSiteFolder, requestFolder, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
