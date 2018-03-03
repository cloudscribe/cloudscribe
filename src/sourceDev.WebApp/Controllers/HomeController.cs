using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using cloudscribe.Web.Common.Analytics;
using sourceDev.WebApp.ViewModels;
using cloudscribe.Messaging.Email;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Razor;

namespace sourceDev.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            SiteContext currentSite,
            IEmailSenderResolver emailSenderResolver,
            ViewRenderer viewRenderer,
            GoogleAnalyticsHelper analyticsHelper
            )
        {
            _currentSite = currentSite;
            _emailSenderResolver = emailSenderResolver;
            _viewRenderer = viewRenderer;
            _analyticsHelper = analyticsHelper;
        }

        private SiteContext _currentSite;
        private IEmailSenderResolver _emailSenderResolver;
        private ViewRenderer _viewRenderer;
        private GoogleAnalyticsHelper _analyticsHelper;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            //AddAnayticsTransaction();

            return View();
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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Test1()
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

            string[] attachmentPaths = null;
            if(!string.IsNullOrWhiteSpace(model.AttachmentFilePathsCsv))
            {
                attachmentPaths = model.AttachmentFilePathsCsv.Split(',');
            }
            


            await sender.SendEmailAsync(
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
                attachmentFilePaths: attachmentPaths,
                configLookupKey: model.ConfigLookupKey


                ).ConfigureAwait(false);

            this.AlertSuccess("message sent", true);

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
