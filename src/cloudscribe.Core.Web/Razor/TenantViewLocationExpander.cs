// by Bed Foster License:MIT
// http://benfoster.io/blog/asp-net-core-themes-and-multi-tenancy
// https://github.com/saaskit/saaskit/blob/master/samples/AspNetMvcSample/TenantViewLocationExpander.cs
// added here 2016-02-12 by Joe Audette

using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web
{
    public class TenantViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "theme", TENANT_KEY = "tenant";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //context.Values[THEME_KEY]
            //    = context.ActionContext.HttpContext.GetTenant<AppTenant>()?.Theme;

            //context.Values[TENANT_KEY]
            //    = context.ActionContext.HttpContext.GetTenant<AppTenant>()?.Name.Replace(" ", "-");
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme = null;
            if (context.Values.TryGetValue(THEME_KEY, out theme))
            {
                IEnumerable<string> themeLocations = new[]
                {
                    $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Shared/{{0}}.cshtml"
                };

                string tenant;
                if (context.Values.TryGetValue(TENANT_KEY, out tenant))
                {
                    themeLocations = ExpandTenantLocations(tenant, themeLocations);
                }

                viewLocations = themeLocations.Concat(viewLocations);
            }


            return viewLocations;
        }

        private IEnumerable<string> ExpandTenantLocations(string tenant, IEnumerable<string> defaultLocations)
        {
            foreach (var location in defaultLocations)
            {
                yield return location.Replace("{0}", $"{{0}}_{tenant}");
                yield return location;
            }
        }
    }
}
