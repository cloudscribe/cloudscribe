using cloudscribe.Core.Models.Site;
using cloudscribe.Core.Web.ViewModels.Account;

namespace cloudscribe.Core.Web.Components
{
    public interface IEmailValidationService
    {
        EmailValidation RegisterEmailValidation(RegisterViewModel model);
    }
}
