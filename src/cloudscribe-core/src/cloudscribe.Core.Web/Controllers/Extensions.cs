// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-02
// Last Modified:			2015-07-18
// 


using cloudscribe.Core.Web.Models;
using cloudscribe.Core.Web.ViewModels.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// to use extensions from inside a controller you have to use the this keyword to call the extension method
    /// http://stackoverflow.com/questions/12105869/controller-extension-method-without-this
    /// </summary>
    public static class Extensions
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
            
            if (controller.Context.GetFeature<ISessionFeature>() != null)
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
}
