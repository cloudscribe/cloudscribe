using cloudscribe.Core.Models;
using cloudscribe.Integration.Tests.TestUtilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;

namespace cloudscribe.Core.Web.Middleware.EnforceSiteRulesMiddleware.Tests
{
    public class TermsOfUseApiTests : IClassFixture<WebApplicationFactory<sourceDev.WebApp.Startup>>
    {
        private readonly WebApplicationFactory<sourceDev.WebApp.Startup> _factory;

        public TermsOfUseApiTests(WebApplicationFactory<sourceDev.WebApp.Startup> factory)
        {
            _factory = factory;
        }


        /// <summary>
        /// Middleware should catch requests to the API when the user has not agreed to Terms of Use, 
        /// and return a 403 Forbidden response
        /// </summary>
        [Fact]
        public async Task ApiRequest_ReturnsForbidden_WhenTermsNotAgreed()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // IP service mock
                    var mockIpService = new Mock<Web.Components.IPService.IBlockedOrPermittedIpService>();
                    mockIpService.Setup(x => x.IsBlockedOrPermittedIp(It.IsAny<IPAddress>(), It.IsAny<Guid>())).Returns(false);
                    services.AddScoped(_ => mockIpService.Object);

                    // User resolver mock, we want to simulate a user who has not accepted the terms of use
                    var mockUserResolver = new Mock<IUserContextResolver>();
                    mockUserResolver.Setup(x => x.GetCurrentUser(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new UserContext(new SiteUser
                                    {
                                        Id       = Guid.NewGuid(),
                                        SiteId   = Guid.NewGuid(),
                                        UserName = "testuser",
                                        Email    = "test@example.com",
                                        AgreementAcceptedUtc = null // Not agreed
                                    }));
                    services.AddScoped(_ => mockUserResolver.Object);

                    // Mock SiteContext with terms of use
                    var siteSettings = new SiteSettings
                    {
                        RegistrationAgreement = "Some agreement text",
                        TermsUpdatedUtc = DateTime.UtcNow
                    };
                    var siteContext = new SiteContext(siteSettings);
                    services.AddScoped(_ => siteContext);

                    // Mock authentication handler
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

                    // Ensure the default authentication scheme is set
                    services.PostConfigure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    });
                });

            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });

            // we need an IP address so we don't get blocked by the IP middleware
            client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");

            // Do it (finally!)
            HttpResponseMessage response = await client.GetAsync("/api/testauth");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            // Ensure Content is not null
            Assert.NotNull(response.Content);

            // Ensure ContentType is not null
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            // Read & deserialise the response body
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TestResponse>(responseBody);
            Assert.NotNull(result);
            Assert.Equal("Forbidden: user has not accepted the terms of use.", result.error);
        }


        /// <summary>
        /// Middleware should allow user through when they have agreed to Terms of Use
        /// </summary>
        [Fact]
        public async Task ApiRequest_ReturnsOK_WhenTermsAgreed()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // IP service mock
                    var mockIpService = new Mock<Web.Components.IPService.IBlockedOrPermittedIpService>();
                    mockIpService.Setup(x => x.IsBlockedOrPermittedIp(It.IsAny<IPAddress>(), It.IsAny<Guid>())).Returns(false);
                    services.AddScoped(_ => mockIpService.Object);

                    // User resolver mock, we want to simulate a user who HAS accepted the terms of use
                    var mockUserResolver = new Mock<IUserContextResolver>();
                    mockUserResolver.Setup(x => x.GetCurrentUser(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new UserContext(new SiteUser
                                    {
                                        Id = Guid.NewGuid(),
                                        SiteId = Guid.NewGuid(),
                                        UserName = "testuser",
                                        Email = "test@example.com",
                                        AgreementAcceptedUtc = DateTime.UtcNow // agreed
                                    }));
                    services.AddScoped(_ => mockUserResolver.Object);

                    // Mock SiteContext with terms of use
                    var siteSettings = new SiteSettings
                    {
                        RegistrationAgreement = "Some agreement text",
                        TermsUpdatedUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) // terms updated recently
                    };
                    var siteContext = new SiteContext(siteSettings);
                    services.AddScoped(_ => siteContext);

                    // Mock authentication handler
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

                    // Ensure the default authentication scheme is set
                    services.PostConfigure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    });
                });

            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });

            // we need an IP address so we don't get blocked by the IP middleware
            client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");

            // Do it (finally!)
            HttpResponseMessage response = await client.GetAsync("/api/testauth");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Ensure Content is not null
            Assert.NotNull(response.Content);

            // Ensure ContentType is not null
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            // Read & deserialise the response body 
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TestResponse>(responseBody);
            Assert.NotNull(result);
            Assert.Equal("Authenticated to test controller", result.message);
        }

        private class TestResponse
        {
            public string? message { get; set; }
            public string? error   { get; set; }
        }
    }
}
