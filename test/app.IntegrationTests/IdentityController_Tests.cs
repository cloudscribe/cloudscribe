using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace app.IntegrationTests
{
    public class IdentityController_Tests : IdServerApiTestBase
    {
        public IdentityController_Tests(TestIdServerWebApplicationFactory<sourceDev.WebApp.Startup> factory):base(factory)
        {

        }

        [Theory]
        [InlineData("/.well-known/openid-configuration")]
        public async Task T10000_CanReachDiscoveryEndpoint(string url)
        {
            // Arrange
            var client = GetUnauthenticatedClient();

            // Act
            var response = await client.GetAsync(url);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/Identity")]
        public async Task T10060_GetShouldNotAllowAnonymousUser(string url)
        {
            //Arrange
            var client = GetUnauthenticatedClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/Identity")]
        public async Task T10070_GetShouldReturnOkForAdmin(string url)
        {
            //Arrange
            var client = await GetAuthenticatedClient("admin@admin.com");

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


    }
}
