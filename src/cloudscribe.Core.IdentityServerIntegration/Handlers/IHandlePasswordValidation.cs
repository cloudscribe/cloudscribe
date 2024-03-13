using cloudscribe.Core.Models;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Handlers
{
    public interface IHandlePasswordValidation<TUser> where TUser : SiteUser
    {
        Task HandlePasswordValidationSuccess(TUser user);
    }

    public class NoopHandlePasswordValidation<TUser> : IHandlePasswordValidation<TUser> where TUser : SiteUser
    {
        public Task HandlePasswordValidationSuccess(TUser user)
        {
            return Task.CompletedTask;
        }
    }
}
