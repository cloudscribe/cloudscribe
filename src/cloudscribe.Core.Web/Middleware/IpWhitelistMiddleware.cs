using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class IpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;

        public IpWhitelistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWhitelistService whitelistService)
        {
            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            SiteContext tenant = context.GetTenant<SiteContext>();
            bool isWhitelisted = whitelistService.IsWhitelisted(remoteIp!, tenant.Id);

            if (!isWhitelisted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }
    }
}