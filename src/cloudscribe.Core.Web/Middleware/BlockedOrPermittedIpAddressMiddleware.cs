using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components.IPService;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class BlockedOrPermittedIpAddressMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockedOrPermittedIpAddressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBlockedOrPermittedIpService blockedOrPermittedIpService)
        {
            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            SiteContext tenant = context.GetTenant<SiteContext>();
            bool isBlocked = blockedOrPermittedIpService.IsBlockedOrPermittedIp(remoteIp!, tenant.Id);

            if (isBlocked)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                return;
            }

            await _next.Invoke(context);
        }
    }
}