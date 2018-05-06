using cloudscribe.Web.Common.Http;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Recaptcha
{
    public class RecaptchaServerSideValidator : IRecaptchaServerSideValidator
    {
        public RecaptchaServerSideValidator(
            IHttpClientProvider httpClientFactory
            )
        {
            _httpClientFactory = httpClientFactory;
        }

        private IHttpClientProvider _httpClientFactory;

        public async Task<RecaptchaResponse> ValidateRecaptcha(
            HttpRequest request,
            string secretKey)
        {
            var client = _httpClientFactory.GetOrCreateHttpClient(new Uri("https://www.google.com/"));

            var response = request.Form["g-recaptcha-response"];
           
            string result = await client.GetStringAsync(
                string.Format("recaptcha/api/siteverify?secret={0}&response={1}",
                    secretKey,
                    response)
                    );

            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

            return captchaResponse;
            
        }

    }
}
