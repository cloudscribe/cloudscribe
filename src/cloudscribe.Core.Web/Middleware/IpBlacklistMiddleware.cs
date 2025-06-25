using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class IpBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        

        public IpBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBlacklistService blacklistService)
        {
            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            SiteContext tenant = context.GetTenant<SiteContext>();
            bool isBlacklisted = blacklistService.IsBlacklisted(remoteIp!, tenant.Id);

            if (isBlacklisted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }
    }
}