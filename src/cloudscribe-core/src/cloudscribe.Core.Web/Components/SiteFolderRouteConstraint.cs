

using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private string requiredSiteFolder;

        public SiteFolderRouteConstraint(string folderParam)
        {
            requiredSiteFolder = folderParam;
        }

        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string parameterName,
            IDictionary<string,object> values,
            RouteDirection routeDirection)
        {
            string requestFolder = RequestSiteResolver.GetFirstFolderSegment(httpContext.Request.Path);
            return string.Equals(requiredSiteFolder, requestFolder, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
