using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using cloudscribe.Web.Common.Analytics;

namespace sourceDev.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(GoogleAnalyticsHelper analyticsHelper)
        {
            _analyticsHelper = analyticsHelper;
        }

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
