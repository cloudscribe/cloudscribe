using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components.IPService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class BlockedOrPermittedIpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _enableIpRestrictions;

        public BlockedOrPermittedIpAddressMiddleware(
            RequestDelegate next,
            IOptions<SiteConfigOptions> configOptions)
        {
            _next = next;
            _enableIpRestrictions = configOptions.Value.EnableIpAddressRestrictions;
        }

        public async Task Invoke(HttpContext context, IBlockedOrPermittedIpService blockedOrPermittedIpService)
        {
            // Fast path: skip entirely if IP restrictions are disabled
            if (!_enableIpRestrictions)
            {
                await _next.Invoke(context);
                return;
            }

            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            SiteContext tenant = context.GetTenant<SiteContext>();
            bool isBlocked = await blockedOrPermittedIpService.IsBlockedOrPermittedIpAsync(remoteIp!, tenant.Id, context.RequestAborted);

            if (isBlocked)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                return;
            }

            await _next.Invoke(context);
        }
    }
}