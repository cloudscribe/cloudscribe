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
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SiteUserManager<SiteUser> _userManager;

        public async Task<IUserContext> GetCurrentUser(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;
            var userId = context.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserContext(user);
            
        }

        public async Task<IUserContext> GetUserById(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserContext(user);

        }

        public async Task<IUserContext> GetUserByEmail(string emailAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user == null) return null;

            return new UserContext(user);

        }

    }
}
