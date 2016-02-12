// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-02
// Last Modified:			2016-02-12
// 

//using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Models;
//using cloudscribe.Core.Web.ViewModels.Common;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Extensions
{
    /// <summary>
    /// to use extensions from inside a controller you have to use the this keyword to call the extension method
    /// http://stackoverflow.com/questions/12105869/controller-extension-method-without-this
    /// </summary>
    public static class ControllerExtensions
    {
        public static async Task<RecaptchaResponse> ValidateRecaptcha(
            this Controller controller,
            HttpRequest request,
            string secretKey)
        {
            var response = request.Form["g-recaptcha-response"];
            var client = new HttpClient();
            string result = await client.GetStringAsync(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    secretKey,
                    response)
                    );

            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

            return captchaResponse;
        }

        public static bool SessionIsAvailable(this Controller controller)
        {
            var sessionFeature = controller.HttpContext.Features.Get<ISessionFeature>();
            return (sessionFeature != null);
        }

        //public static RedirectResult RedirectToSiteRoot(this Controller controller, ISiteSettings site)
        //{
        //    if(site.SiteFolderName.Length > 0)
        //    {
        //        return controller.Redirect("/" + site.SiteFolderName);
        //    }

        //    return controller.Redirect("/");
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
            
            if (controller.SessionIsAvailable())
            {
                var alerts = controller.TempData.GetAlerts();

                alerts.Add(new Alert
                {
                    AlertStyle = alertStyle,
                    Message = message,
                    Dismissable = dismissable
                });

                controller.TempData.AddAlerts(alerts);
            }

        }

        public static void AddAlerts(this ITempDataDictionary tempData, List<Alert> alerts)
        {
            tempData[Alert.TempDataKey] = JsonConvert.SerializeObject(alerts);
        }

        public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
        {
            if(tempData.ContainsKey(Alert.TempDataKey))
            {
                string json = (string)tempData[Alert.TempDataKey];
                List<Alert> alerts = JsonConvert.DeserializeObject<List<Alert>>(json);
                return alerts;
            }

            return new List<Alert>();
        }

        //public static IActionResult RedirectToLocal(this Controller controller, string returnUrl)
        //{
        //    if (controller.Url.IsLocalUrl(returnUrl))
        //    {
        //        return controller.Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return controller.RedirectToAction("Index", "Home");
        //    }
        //}

        


    }
}
