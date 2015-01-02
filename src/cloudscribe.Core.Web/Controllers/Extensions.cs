// Author:					Joe Audette
// Created:					2015-01-02
// Last Modified:			2015-01-02
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Common;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// to use extensions from inside a controller you have to use the this keyword to call the extension method
    /// http://stackoverflow.com/questions/12105869/controller-extension-method-without-this
    /// </summary>
    public static class Extensions
    {
        public static ISiteContext GetSiteContext(this Controller controller)
        {
            return controller.HttpContext.GetOwinContext().Get<ISiteContext>();
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
