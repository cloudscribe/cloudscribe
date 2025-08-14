using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace cloudscribe.Core.Web.Controllers.PrivacyControllerTests
{
    public class PrivacyControllerTests
    {
        private PrivacyController CreateController(
            Mock<ITrackingConsentFeature>? consentFeature = null,
            Mock<HttpContext>? httpContext = null,
            Mock<IResponseCookies>? responseCookies = null,
            bool isHttps = true)
        {
            // Create SiteContext with SiteSettings
            var siteSettings = new SiteSettings 
            { 
                Id = Guid.NewGuid(),
                SiteName = "Test Site",
                SiteFolderName = ""
            };
            var siteContext = new SiteContext(siteSettings);

            // Create controller
            var controller = new PrivacyController(siteContext);

            // Setup HttpContext
            var context = httpContext ?? new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            var cookies = responseCookies ?? new Mock<IResponseCookies>();
            var features = new Mock<IFeatureCollection>();
            
            // Setup request
            request.Setup(r => r.IsHttps).Returns(isHttps);
            request.Setup(r => r.Scheme).Returns(isHttps ? "https" : "http");
            request.Setup(r => r.Host).Returns(new HostString("localhost"));
            request.Setup(r => r.PathBase).Returns(new PathString(""));
            
            // Setup response cookies
            response.Setup(r => r.Cookies).Returns(cookies.Object);
            
            // Setup features
            if (consentFeature != null)
            {
                features.Setup(f => f.Get<ITrackingConsentFeature>())
                    .Returns(consentFeature.Object);
            }
            
            // Wire everything together
            context.Setup(c => c.Request).Returns(request.Object);
            context.Setup(c => c.Response).Returns(response.Object);
            context.Setup(c => c.Features).Returns(features.Object);
            
            // Setup TempData (required for RedirectToSiteRoot)
            var tempData = new Mock<ITempDataDictionary>();
            controller.TempData = tempData.Object;
            
            // Set the controller context
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context.Object
            };

            return controller;
        }

        #region Index Action Tests

        [Fact]
        public void Index_ReturnsViewWithSiteContext()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SiteContext>(result.Model);
            Assert.Equal("Test Site", ((SiteContext)result.Model).SiteName);
        }

        #endregion

        #region WithdrawCookieConsent Tests

        [Fact]
        public void WithdrawCookieConsent_CallsWithdrawConsent_WhenFeatureExists()
        {
            // Arrange
            var consentFeature = new Mock<ITrackingConsentFeature>();
            consentFeature.Setup(f => f.WithdrawConsent());
            var controller = CreateController(consentFeature);

            // Act
            var result = controller.WithdrawCookieConsent() as RedirectResult;

            // Assert
            consentFeature.Verify(f => f.WithdrawConsent(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("/", result.Url); // RedirectToSiteRoot with empty folder
        }

        [Fact]
        public void WithdrawCookieConsent_HandlesNullConsentFeature()
        {
            // Arrange
            var controller = CreateController(consentFeature: null);

            // Act
            var result = controller.WithdrawCookieConsent() as RedirectResult;

            // Assert - Should not throw exception
            Assert.NotNull(result);
            Assert.Equal("/", result.Url);
        }

        #endregion

        #region ShowCookieBanner Tests

        [Fact]
        public void ShowCookieBanner_DeletesDismissCookie()
        {
            // Arrange
            var responseCookies = new Mock<IResponseCookies>();
            var controller = CreateController(responseCookies: responseCookies);

            // Act
            var result = controller.ShowCookieBanner() as RedirectResult;

            // Assert
            responseCookies.Verify(c => c.Delete("cookieconsent_dismissed"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("/", result.Url);
        }

        #endregion

        #region ResetCookiePreferences Tests

        [Fact]
        public void ResetCookiePreferences_WithdrawsConsent_And_DeletesDismissCookie_WithHttps()
        {
            // Arrange
            var consentFeature = new Mock<ITrackingConsentFeature>();
            var responseCookies = new Mock<IResponseCookies>();
            var controller = CreateController(consentFeature, responseCookies: responseCookies, isHttps: true);

            // Act
            var result = controller.ResetCookiePreferences() as RedirectResult;

            // Assert
            consentFeature.Verify(f => f.WithdrawConsent(), Times.Once);
            
            responseCookies.Verify(c => c.Delete(
                "cookieconsent_dismissed",
                It.Is<CookieOptions>(opts =>
                    opts.Path == "/" &&
                    opts.SameSite == SameSiteMode.Lax &&
                    opts.HttpOnly == false &&
                    opts.Secure == true // HTTPS
                )), Times.Once);
            
            Assert.NotNull(result);
            Assert.Equal("/", result.Url);
        }

        [Fact]
        public void ResetCookiePreferences_WithdrawsConsent_And_DeletesDismissCookie_WithHttp()
        {
            // Arrange
            var consentFeature = new Mock<ITrackingConsentFeature>();
            var responseCookies = new Mock<IResponseCookies>();
            var controller = CreateController(consentFeature, responseCookies: responseCookies, isHttps: false);

            // Act
            var result = controller.ResetCookiePreferences() as RedirectResult;

            // Assert
            consentFeature.Verify(f => f.WithdrawConsent(), Times.Once);
            
            responseCookies.Verify(c => c.Delete(
                "cookieconsent_dismissed",
                It.Is<CookieOptions>(opts =>
                    opts.Path == "/" &&
                    opts.SameSite == SameSiteMode.Lax &&
                    opts.HttpOnly == false &&
                    opts.Secure == false // HTTP
                )), Times.Once);
            
            Assert.NotNull(result);
        }

        [Fact]
        public void ResetCookiePreferences_HandlesNullConsentFeature()
        {
            // Arrange
            var responseCookies = new Mock<IResponseCookies>();
            var controller = CreateController(consentFeature: null, responseCookies: responseCookies);

            // Act
            var result = controller.ResetCookiePreferences() as RedirectResult;

            // Assert - Should still delete dismiss cookie even if consent feature is null
            responseCookies.Verify(c => c.Delete(
                "cookieconsent_dismissed",
                It.IsAny<CookieOptions>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.Equal("/", result.Url);
        }

        #endregion

        #region Multi-Tenant Redirect Tests

        [Fact]
        public void AllActions_RedirectToCorrectPath_ForTenantWithFolder()
        {
            // Arrange
            var siteSettings = new SiteSettings
            {
                Id = Guid.NewGuid(),
                SiteName = "Tenant Site",
                SiteFolderName = "tenant1"
            };
            var siteContext = new SiteContext(siteSettings);
            
            var controller = new PrivacyController(siteContext);
            
            // Setup minimal HttpContext for redirect
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            var cookies = new Mock<IResponseCookies>();
            
            request.Setup(r => r.Scheme).Returns("https");
            request.Setup(r => r.Host).Returns(new HostString("localhost"));
            request.Setup(r => r.PathBase).Returns(new PathString("/tenant1"));
            response.Setup(r => r.Cookies).Returns(cookies.Object);
            context.Setup(c => c.Request).Returns(request.Object);
            context.Setup(c => c.Response).Returns(response.Object);
            context.Setup(c => c.Features).Returns(new Mock<IFeatureCollection>().Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;

            // Act
            var result = controller.ShowCookieBanner() as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("/tenant1", result.Url); // Should redirect to tenant folder
        }

        #endregion

        #region ValidateAntiForgeryToken Tests

        [Fact]
        public void ResetCookiePreferences_HasValidateAntiForgeryTokenAttribute()
        {
            // Arrange
            var controller = CreateController();
            var methodInfo = typeof(PrivacyController).GetMethod(nameof(PrivacyController.ResetCookiePreferences));

            // Act
            var attributes = methodInfo?.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false);

            // Assert
            Assert.NotNull(attributes);
            Assert.Single(attributes);
        }

        [Fact]
        public void ShowCookieBanner_DoesNotHaveValidateAntiForgeryTokenAttribute()
        {
            // This action doesn't modify critical state, so doesn't need CSRF protection
            var methodInfo = typeof(PrivacyController).GetMethod(nameof(PrivacyController.ShowCookieBanner));
            var attributes = methodInfo?.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false);
            
            Assert.NotNull(attributes);
            Assert.Empty(attributes);
        }

        #endregion

        #region HTTP Method Tests

        [Fact]
        public void AllCookieActions_RequireHttpPost()
        {
            // Verify that all cookie manipulation actions require POST
            var withdrawMethod = typeof(PrivacyController).GetMethod(nameof(PrivacyController.WithdrawCookieConsent));
            var showBannerMethod = typeof(PrivacyController).GetMethod(nameof(PrivacyController.ShowCookieBanner));
            var resetMethod = typeof(PrivacyController).GetMethod(nameof(PrivacyController.ResetCookiePreferences));

            Assert.NotNull(withdrawMethod?.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault());
            Assert.NotNull(showBannerMethod?.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault());
            Assert.NotNull(resetMethod?.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault());
        }

        #endregion
    }
}