// Author:					Joe Audette
// Created:					2015-01-02
// Last Modified:			2015-06-11
// 

//using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Models;
using cloudscribe.Core.Web.ViewModels.Common;
//using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
//using System;
using System.Collections.Generic;
//using System.Net.Http;  //2015-06-11 this seems available on dnxcore50 but not dnx451 though the package reference exists in both
                            // seems like a packaging error from MS, the package doesn't list dnx451 as supported so vs gives an error

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// to use extensions from inside a controller you have to use the this keyword to call the extension method
    /// http://stackoverflow.com/questions/12105869/controller-extension-method-without-this
    /// </summary>
    public static class Extensions
    {
        //public static async Task<RecaptchaResponse> ValidateRecaptcha(
        //    this Controller controller,
        //    HttpRequest request,
        //    string secretKey)
        //{
        //    var response = request.Form["g-recaptcha-response"];
        //    var client = new HttpClient();
        //    string result = await client.GetStringAsync(
        //        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
        //            secretKey,
        //            response)
        //            );

        //    var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

        //    return captchaResponse;
        //}

        //public static ISiteContext GetSiteContext(this Controller controller)
        //{
        //    return controller.HttpContext.GetOwinContext().Get<ISiteContext>();
        //}

        public static void AlertSuccess(
            this Controller controller,
            string message,
            bool dismissable = true)
        {
            controller.AddAlert(AlertStyles.Success, message, dismissable);
        }

        public static void AlertInformation(
            this Controller controller,
            string message,
            bool dismissable = false)
        {
            controller.AddAlert(AlertStyles.Information, message, dismissable);
        }

        public static void AlertWarning(
            this Controller controller,
            string message,
            bool dismissable = false)
        {
            controller.AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public static void AlertDanger(
            this Controller controller,
            string message,
            bool dismissable = false)
        {
            controller.AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private static void AddAlert(
            this Controller controller,
            string alertStyle,
            string message,
            bool dismissable)
        {
            var alerts = controller.TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)controller.TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            controller.TempData[Alert.TempDataKey] = alerts;
        }

    }
}
