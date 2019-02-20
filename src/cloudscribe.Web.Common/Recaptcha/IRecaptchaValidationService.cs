using cloudscribe.Web.Common.Models;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Recaptcha
{
    public interface IRecaptchaValidationService
    {
        Task<RecaptchaResponse> ValidateRecaptcha(string recaptchaResponse, string secretKey);
    }
}
