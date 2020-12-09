using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public interface IEmailChangeHandler
    {
        Task<bool> HandleEmailChangeWithoutUserConfirmation(ChangeUserEmailViewModel model, SiteUser user, string token, string callbackUrl);
        Task<bool> HandleEmailChangeWithUserConfirmation(ChangeUserEmailViewModel model, SiteUser user, string token, string confirmationUrl, string siteUrl);
        Task<bool> HandleEmailChangeConfirmation(ChangeUserEmailViewModel model, SiteUser user, string newEmail, string token, string siteUrl);
    }
}