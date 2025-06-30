using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class IpPermittedMiddleware
    {
        private readonly RequestDelegate _next;

        public IpPermittedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IPermittedIpService permittedIpService)
        {
            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            SiteContext tenant = context.GetTenant<SiteContext>();
            bool isPermitted = permittedIpService.IsPermittedIp(remoteIp!, tenant.Id);

            if (!isPermitted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }
    }
}