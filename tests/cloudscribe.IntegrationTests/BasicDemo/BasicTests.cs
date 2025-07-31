using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

// Demo of how to set up a WebApplication & Client and run it against various endpoints 
// in the DevWebApp - note that such requests go via the middleware,
// so our client needs to state its IP address - for which we need ForwardedHeaders:
// see startup.cs line: if (env.IsDevelopment()) app.UseForwardedHeaders();
// and must also deal with https requirement.
// These tests seem s-l-o-w to spin up - but do work in the end -- jk


namespace cloudscribe.IntegrationTests.BasicDemo
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
            HttpClient client = CreateTestClient();

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
            HttpClient client = CreateTestClient();

            // Act
            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private HttpClient CreateTestClient()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                // if we want these to run on the CI server, might need to consider what env to use
                // builder.UseEnvironment("Development");  

                builder.ConfigureTestServices(services =>
                {
                    services.PostConfigure<ForwardedHeadersOptions>(options =>
                    {
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                    });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost") // Force HTTPS
            });
            client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");
            return client;
        }
    }
}
