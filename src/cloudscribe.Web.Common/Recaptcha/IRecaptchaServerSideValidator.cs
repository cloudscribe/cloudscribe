using System.Threading.Tasks;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Http;

namespace cloudscribe.Web.Common.Recaptcha
{
    public interface IRecaptchaServerSideValidator
    {
        Task<RecaptchaResponse> ValidateRecaptcha(HttpRequest request, string secretKey);
    }
}