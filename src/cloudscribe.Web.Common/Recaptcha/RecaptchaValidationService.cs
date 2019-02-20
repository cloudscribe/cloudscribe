using cloudscribe.Web.Common.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Recaptcha
{
    public class RecaptchaValidationService : IRecaptchaValidationService
    {
        public RecaptchaValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly HttpClient _httpClient;

        public async Task<RecaptchaResponse> ValidateRecaptcha(
           string recaptchaResponse,
           string secretKey)
        {
            string result = await _httpClient.GetStringAsync(
                string.Format("recaptcha/api/siteverify?secret={0}&response={1}",
                    secretKey,
                    recaptchaResponse)
                    );

            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

            return captchaResponse;

        }

    }
}
