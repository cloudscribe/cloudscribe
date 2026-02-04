using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace app.IntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<sourceDev.WebApp.Startup>>
    {
        private readonly WebApplicationFactory<sourceDev.WebApp.Startup> _factory;

        public BasicTests(WebApplicationFactory<sourceDev.WebApp.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/home/about")]
        [InlineData("/privacy")]
        [InlineData("/account/login")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/.well-known/openid-configuration")]
        public async Task CanReachDiscoveryEndpoint(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            //var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
