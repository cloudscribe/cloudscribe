using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace app.IntegrationTests
{
    public class IdServerApiTestBase : IClassFixture<TestIdServerWebApplicationFactory<sourceDev.WebApp.Startup>>
    {
        private readonly TestIdServerWebApplicationFactory<sourceDev.WebApp.Startup> _factory;


        public IdServerApiTestBase(TestIdServerWebApplicationFactory<sourceDev.WebApp.Startup> factory)
        {
            _factory = factory;
        }

        public HttpClient GetUnauthenticatedClient()
        {
            return _factory.CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClient(string userName = "admin@admin.com", string password = "admin")
        {
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var invoker = client as HttpMessageInvoker;
            var tokenRequest = new PasswordTokenRequest
            {
                Address = "http://localhost/connect/token",
                ClientId = "Test",
                ClientSecret = "secret",
                Scope = "idserverapi",
                UserName = userName,
                Password = password
            };

            var tokenResponse = await invoker.RequestPasswordTokenAsync(tokenRequest);

            client.SetBearerToken(tokenResponse.AccessToken);
            return client;
        }
    }
}
