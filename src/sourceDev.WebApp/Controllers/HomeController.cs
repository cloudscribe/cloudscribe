using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using cloudscribe.Web.Common.Analytics;
using sourceDev.WebApp.ViewModels;
using cloudscribe.Email;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using cloudscribe.Core.Identity;

namespace sourceDev.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            IOidcHybridFlowHelper oidcHybridFlowHelper,
            IHttpClientFactory httpClientFactory,
            IdentityServer4.IdentityServerTools idserver,
            SiteContext currentSite,
            IEmailSenderResolver emailSenderResolver,
            ViewRenderer viewRenderer,
            GoogleAnalyticsHelper analyticsHelper
            )
        {
            _oidcHybridFlowHelper = oidcHybridFlowHelper;
            _currentSite = currentSite;
            _emailSenderResolver = emailSenderResolver;
            _viewRenderer = viewRenderer;
            _analyticsHelper = analyticsHelper;
            _httpClientFactory = httpClientFactory;
            _idserver = idserver;
        }

        private readonly IOidcHybridFlowHelper _oidcHybridFlowHelper;
        private SiteContext _currentSite;
        private IEmailSenderResolver _emailSenderResolver;
        private ViewRenderer _viewRenderer;
        private GoogleAnalyticsHelper _analyticsHelper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IdentityServer4.IdentityServerTools _idserver;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";
            //AddAnayticsTransaction();
            var client = _httpClientFactory.CreateClient();
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri("https://localhost:44399/api/identity");
            
            var token = await _oidcHybridFlowHelper.GetAccessToken(User);

            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("Authorization", "Bearer " + token);
            }



            var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {

                }
            }
            

            return View();
        }

        //[HttpGet]
        //public IActionResult GetAlerts()
        //{
        //    var alerts = TempData.GetAlerts();


        //    return Ok(alerts);
        //}


        [HttpGet]
        public IActionResult DateTest()
        {
            var model = new DateValidationTestViewModel();


            return View(model);
        }

        [HttpPost]
        public IActionResult DateTest(DateValidationTestViewModel model)
        {

            this.AlertSuccess("Success", true);

            return View(model);
        }

        private void AddAnayticsTransaction()
        {
            //https://developers.google.com/analytics/devguides/collection/analyticsjs/ecommerce

            var transaction = new Transaction("1234");
            transaction.Affilitation = "Acme Clothing";
            transaction.Revenue = 11.99M;
            transaction.Shipping = 5;
            transaction.Tax = 1.29M;
            transaction.CurrencyCode = "EUR";

            var item = new TransactonItem("1234", "Fluffy Pink Bunnies");
            item.Sku = "DD23444";
            item.Category = "Party Toys";
            item.Price = 11.99M;
            item.Quantity = 1;
            item.CurrencyCode = "GDP";
            transaction.Items.Add(item);
            _analyticsHelper.AddTransaction(transaction);
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "Your contact page.";

            var claims = User.Claims.ToList();

            var accessToken = await HttpContext.GetTokenAsync(IdentityConstants.ExternalScheme, "access_token");
            

            return View();
        }

        public IActionResult Test1()
        {
            

            return View();
        }

        public IActionResult Test2()
        {


            return View();
        }

        public IActionResult Test3()
        {


            return View();
        }

        public IActionResult Map()
        {
            return View();
        }

        public IActionResult EditTest()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TestEmail()
        {
            var model = new TestSendEmailViewModel();
            model.Subject = "Testing Email Providers";
            model.ConfigLookupKey = _currentSite.Id.ToString();

            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TestEmail(TestSendEmailViewModel model)
        {
            var sender = await _emailSenderResolver.GetEmailSender(model.ConfigLookupKey);
            var messageModel = new TestEmailMessageViewModel
            {
                Tenant = _currentSite,
                Greeting = "Hey there from " + sender.Name,
                Message = model.Message
            };

            var htmlMessage
                    = await _viewRenderer.RenderViewAsString<TestEmailMessageViewModel>("TestEmailMessage", messageModel).ConfigureAwait(false);

            if (model.AttachmentFilePathsCsv == "joetest")
            {
                model.AttachmentFilePathsCsv = @"C:\_c\cloudscribe\src\sourceDev.WebApp\wwwroot\testfiles\PowerShell_Examples_v4.pdf,C:\_c\cloudscribe\src\sourceDev.WebApp\wwwroot\testfiles\Shortcut-Keys-For-Windows-10.docx";

            }

            List<EmailAttachment> attachments = null;
            string[] attachmentPaths = null;
            if(!string.IsNullOrWhiteSpace(model.AttachmentFilePathsCsv))
            {
                attachments = new List<EmailAttachment>();
                attachmentPaths = model.AttachmentFilePathsCsv.Split(',');
                foreach(var path in attachmentPaths)
                {
                    var stream = System.IO.File.OpenRead(path);
                    var attachment = new EmailAttachment(stream, Path.GetFileName(path));
                    attachments.Add(attachment);
                }
            }
            
            var result = await sender.SendEmailAsync(
                model.ToEmailCsv,
                model.FromEmail,
                model.Subject,
                null,
                htmlMessage,
                replyToEmail: model.ReplyToEmail,
                replyToName: model.ReplyToName,
                fromName: model.FromName,
                toAliasCsv:model.ToAliasCsv,
                ccEmailCsv:model.CcEmailCsv,
                ccAliasCsv:model.CcAliasCsv,
                bccEmailCsv:model.BccEmailCsv,
                bccAliasCsv:model.BccAliasCsv,
                attachments: attachments,
                configLookupKey: model.ConfigLookupKey


                ).ConfigureAwait(false);

            if(result.Succeeded)
            {
                this.AlertSuccess("message sent", true);
            }
            else
            {
                this.AlertDanger(result.ErrorMessage, true);
            }
            

            return RedirectToAction("TestEmail");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public IActionResult ClearLanguageCookie(string culture, string returnUrl)
        {
            Response.Cookies.Delete(
                CookieRequestCultureProvider.DefaultCookieName
            );

            return LocalRedirect(returnUrl);
        }

    }
}
