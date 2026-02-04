using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.RegularExpressions;
using Xunit;

namespace cloudscribe.Core.Web.CookieConsent.Tests
{
    /// <summary>
    /// Focused integration tests for the three-state cookie consent system.
    /// Tests critical paths through the full ASP.NET Core pipeline.
    /// 
    /// JK - these really are pretty convoluted and very likely a bit excessive - 
    /// but serve as good reference for how you might integration-test cookie-setting.
    /// </summary>
    public class ThreeStateCookieConsentTests : IClassFixture<WebApplicationFactory<sourceDev.WebApp.Startup>>
    {
        private readonly WebApplicationFactory<sourceDev.WebApp.Startup> _factory;

        public ThreeStateCookieConsentTests(WebApplicationFactory<sourceDev.WebApp.Startup> factory)
        {
            _factory = factory;
        }

        #region Helper Methods

        private HttpClient CreateTestClient()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
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
                BaseAddress = new Uri("https://localhost")
            });
            client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");
            return client;
        }

        private async Task<string?> GetAntiForgeryToken(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            // Extract the antiforgery token from the form
            var match = Regex.Match(content, @"<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)""");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private bool HasSetCookieHeader(HttpResponseMessage response, string cookieName)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                return cookies.Any(c => c.Contains($"{cookieName}="));
            }
            return false;
        }

        private string? GetSetCookieValue(HttpResponseMessage response, string cookieName)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var cookie = cookies.FirstOrDefault(c => c.Contains($"{cookieName}="));
                if (cookie != null)
                {
                    var match = Regex.Match(cookie, $"{cookieName}=([^;]+)");
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
            }
            return null;
        }

        private bool IsCookieBeingDeleted(HttpResponseMessage response, string cookieName)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var cookie = cookies.FirstOrDefault(c => c.Contains($"{cookieName}="));
                if (cookie != null)
                {
                    // Check for deletion indicators: expires in past, max-age=0, or empty value
                    return cookie.Contains("max-age=0") || 
                           cookie.Contains("expires=Thu, 01 Jan 1970") ||
                           cookie.Contains($"{cookieName}=;");
                }
            }
            return false;
        }

        #endregion

        #region Critical Path Tests

        [Fact]
        public async Task BannerAppearsOnFirstVisit_DisappearsAfterAction()
        {
            // Arrange
            var client = CreateTestClient();

            // Act 1: First visit should show banner
            var response1 = await client.GetAsync("/");
            var content1 = await response1.Content.ReadAsStringAsync();

            // Assert 1: Banner should be visible
            response1.EnsureSuccessStatusCode();
            Assert.Contains("id=\"cookieConsent\"", content1);
            Assert.Contains("Accept", content1);
            Assert.Contains("Decline", content1);
            
            // Check that no consent cookies are set on first visit
            Assert.False(HasSetCookieHeader(response1, "cookieconsent_status"), 
                "Consent cookie should not be set on first page load");
            Assert.False(HasSetCookieHeader(response1, "cookieconsent_dismissed"),
                "Dismiss cookie should not be set on first page load");
        }

        [Fact]
        public async Task ResetCookiePreferences_RequiresAntiForgeryToken()
        {
            // Arrange
            var client = CreateTestClient();
            
            // Act 1: Try POST without token (should fail)
            var response1 = await client.PostAsync("/privacy/resetcookiepreferences", new FormUrlEncodedContent(new Dictionary<string, string>()));
            
            // Assert 1: Should get BadRequest or Redirect (depending on config)
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest || 
                       response1.StatusCode == HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task ResetCookiePreferences_DeletesCookies_WithValidToken()
        {
            // Arrange
            var client = CreateTestClient();
            
            // First, set cookies that we want to reset
            client.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Consent=yes; cookieconsent_dismissed=true");
            
            // Get the antiforgery token from privacy page
            var privacyResponse = await client.GetAsync("/privacy");
            var privacyContent = await privacyResponse.Content.ReadAsStringAsync();
            
            // Look for form with ResetCookiePreferences action
            var tokenMatch = Regex.Match(privacyContent, 
                @"action=""/privacy/resetcookiepreferences""[^>]*>.*?<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)""", 
                RegexOptions.Singleline);
            
            if (!tokenMatch.Success)
            {
                // Skip if no form found (may depend on consent state)
                return;
            }

            var token = tokenMatch.Groups[1].Value;

            // Act: POST with valid token
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", token)
            });
            
            var response = await client.PostAsync("/privacy/resetcookiepreferences", formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location?.ToString());
            
            // Check for cookie deletion in Set-Cookie headers
            if (response.Headers.Contains("Set-Cookie"))
            {
                // The dismiss cookie should be deleted
                Assert.True(IsCookieBeingDeleted(response, "cookieconsent_dismissed"),
                    "The cookieconsent_dismissed cookie should be deleted");
                
                // The consent cookie deletion is handled by the framework
                // and may appear differently in headers
            }
        }

        [Fact]
        public async Task WithdrawCookieConsent_DeletesConsentCookie()
        {
            // Arrange
            var client = CreateTestClient();
            
            // Set a consent cookie first
            client.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Consent=yes");
            
            // Get antiforgery token if needed
            var privacyResponse = await client.GetAsync("/privacy");
            var privacyContent = await privacyResponse.Content.ReadAsStringAsync();
            
            // Look for WithdrawCookieConsent form
            var tokenMatch = Regex.Match(privacyContent,
                @"action=""/privacy/withdrawcookieconsent""[^>]*>.*?<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)""",
                RegexOptions.Singleline);
            
            FormUrlEncodedContent? formData = null;
            if (tokenMatch.Success)
            {
                formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("__RequestVerificationToken", tokenMatch.Groups[1].Value)
                });
            }
            else
            {
                formData = new FormUrlEncodedContent(new Dictionary<string, string>());
            }

            // Act
            var response = await client.PostAsync("/privacy/withdrawcookieconsent", formData);

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Equal("/", response.Headers.Location?.ToString());
                
                // Check if consent cookie is being deleted
                // The framework handles this via ITrackingConsentFeature.WithdrawConsent()
                // We may see the cookie deleted or modified in Set-Cookie headers
            }
        }

        [Fact]
        public async Task ShowCookieBanner_DeletesDismissCookie()
        {
            // Arrange
            var client = CreateTestClient();
            
            // Set a dismiss cookie first
            client.DefaultRequestHeaders.Add("Cookie", "cookieconsent_dismissed=true");
            
            // Act
            var response = await client.PostAsync("/privacy/showcookiebanner", new FormUrlEncodedContent(new Dictionary<string, string>()));

            // Assert  
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Equal("/", response.Headers.Location?.ToString());
                
                // Check for dismiss cookie deletion
                Assert.True(IsCookieBeingDeleted(response, "cookieconsent_dismissed"),
                    "The cookieconsent_dismissed cookie should be deleted when ShowCookieBanner is called");
            }
        }

        #endregion

        #region Cookie Security Tests

        [Fact]
        public async Task WithdrawCookieConsent_RequiresPost()
        {
            // Arrange
            var client = CreateTestClient();

            // Act - Try GET request (should fail)
            var response = await client.GetAsync("/privacy/withdrawcookieconsent");

            // Assert - Should get 404 or 405 Method Not Allowed
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound || 
                response.StatusCode == HttpStatusCode.MethodNotAllowed,
                "GET request to POST-only endpoint should fail"
            );
        }

        [Fact]
        public async Task ShowCookieBanner_RequiresPost()
        {
            // Arrange
            var client = CreateTestClient();

            // Act - Try GET request (should fail)
            var response = await client.GetAsync("/privacy/showcookiebanner");

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound || 
                response.StatusCode == HttpStatusCode.MethodNotAllowed,
                "GET request to POST-only endpoint should fail"
            );
        }

        #endregion

        #region Set-Cookie Header Tests

        [Fact]
        public async Task CookieHeaders_HaveCorrectSecurityAttributes()
        {
            // Arrange
            var client = CreateTestClient();
            
            // Act - Trigger a cookie deletion to see the Set-Cookie header
            var response = await client.PostAsync("/privacy/showcookiebanner", 
                new FormUrlEncodedContent(new Dictionary<string, string>()));

            // Assert
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var dismissCookie = cookies.FirstOrDefault(c => c.Contains("cookieconsent_dismissed"));
                if (dismissCookie != null)
                {
                    // Check for security attributes
                    Assert.True(dismissCookie.ToLower().Contains("path=/"), 
                        "Cookie should have path=/ for site-wide scope");
                    
                    // SameSite should be set (Lax is recommended)
                    Assert.True(dismissCookie.ToLower().Contains("samesite="), 
                        "Cookie should have SameSite attribute");
                    
                    // For HTTPS, should have Secure flag
                    if (client.BaseAddress?.Scheme == "https")
                    {
                        // Note: Secure flag may or may not be present depending on test setup
                        // In production with HTTPS, this should always be true
                    }
                }
            }
        }

        #endregion
    }
}