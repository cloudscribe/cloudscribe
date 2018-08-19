using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;

namespace cloudscribe.Core.Identity
{
    public static class SiteCookieConsent
    {
        public static bool NeedsConsent(HttpContext context)
        {
            var tenant = context.GetTenant<SiteContext>();

            if (tenant != null)
            {
                return tenant.RequireCookieConsent;
            }

            return false;
        }
    }
}
