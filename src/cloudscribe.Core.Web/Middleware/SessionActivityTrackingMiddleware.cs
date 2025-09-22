using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class SessionActivityTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionActivityTrackingMiddleware> _logger;

        public SessionActivityTrackingMiddleware(
            RequestDelegate next,
            ILogger<SessionActivityTrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            SiteContext currentSite,
            ISessionActivityService activityService)
        {
            // Process the request
            await _next(context);

            // After request completes, if user is authenticated and request succeeded
            // Do NOT call AuthenticateAsync() - just use the principal that's already there
            // IMPORTANT: Don't track the session-check endpoint as activity!
            var isSessionCheckEndpoint = context.Request.Path.Value?.Contains("/GetActualSessionTime") ?? false;
            
            if (context.User?.Identity?.IsAuthenticated == true &&
                context.Response.StatusCode >= 200 &&
                context.Response.StatusCode < 300 &&
                !isSessionCheckEndpoint)
            {
                // Get user ID (GUID) from claims
                var userId = context.User.FindFirst("sub")?.Value
                            ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && currentSite != null)
                {
                    // Update the cached expiry time using the current site's ID (GUID as string)
                    activityService.UpdateActivity(userId, currentSite.Id.ToString());
                    
                    _logger.LogDebug($"Session activity tracked for user {userId} in site {currentSite.SiteName}");
                }
            }
        }
    }
}