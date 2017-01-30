using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class UserContextResolver : IUserContextResolver
    {
        public UserContextResolver(
            SiteUserManager<SiteUser> userManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SiteUserManager<SiteUser> userManager;

        public async Task<IUserContext> GetCurrentUser(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var context = httpContextAccessor.HttpContext;
            if (context == null) return null;
            var userId = context.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return null;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserContext(user);
            
        }

    }
}
