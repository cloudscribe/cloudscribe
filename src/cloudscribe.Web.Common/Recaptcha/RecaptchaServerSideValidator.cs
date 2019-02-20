using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Recaptcha
{
    
    public class RecaptchaServerSideValidator : IRecaptchaServerSideValidator
    {
        public RecaptchaServerSideValidator(
            IRecaptchaValidationService recaptchaValidationService
            )
        {
            _recaptchaValidationService = recaptchaValidationService;
        }

        private readonly IRecaptchaValidationService _recaptchaValidationService;

        public async Task<RecaptchaResponse> ValidateRecaptcha(
            HttpRequest request,
            string secretKey)
        {
            var response = request.Form["g-recaptcha-response"];
            return await _recaptchaValidationService.ValidateRecaptcha(response, secretKey);
            
        }

    }
}
