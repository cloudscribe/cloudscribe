// Author:					Joe Audette
// Created:					2014-12-30
// Last Modified:			2015-05-07
// 

using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

namespace cloudscribe.Core.Web.Helpers
{
    public static class ModalDialogHelpers
    {
        
        /// <summary>
        /// you must include /Scripts/cloudscribe-modaldialog-bootstrap.js
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ModalDialogActionLinkBootstrap(
            this AjaxHelper ajaxHelper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes
            )
        {
            var dialogDivId = Guid.NewGuid().ToString();
            AjaxOptions options = new AjaxOptions
            {
                UpdateTargetId = dialogDivId,
                InsertionMode = InsertionMode.Replace,
                HttpMethod = "GET",
                OnBegin = string.Format(CultureInfo.InvariantCulture, "prepareModalDialog('{0}')", dialogDivId),
                OnFailure = string.Format(CultureInfo.InvariantCulture, "clearModalDialog('{0}');alert('Ajax call failed')", dialogDivId),
                OnSuccess = string.Format(CultureInfo.InvariantCulture, "openModalDialog('{0}')", dialogDivId)
            };


            return ajaxHelper.ActionLink(
                linkText,
                actionName,
                controllerName,
                routeValues,
                options,
                htmlAttributes
                );
        }

        /// <summary>
        /// you must include /Scripts/modaldialog-jqui.js
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="dialogTitle"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ModalDialogActionLinkJQueryUI(
            this AjaxHelper ajaxHelper, 
            string linkText, 
            string dialogTitle,
            string actionName, 
            string controllerName, 
            object routeValues,
            object htmlAttributes
            )
        {
            var dialogDivId = Guid.NewGuid().ToString();
            AjaxOptions options = new AjaxOptions
                    {
                        UpdateTargetId = dialogDivId,
                        InsertionMode = InsertionMode.Replace,
                        HttpMethod = "GET",
                        OnBegin = string.Format(CultureInfo.InvariantCulture, "prepareModalDialog('{0}')", dialogDivId),
                        OnFailure = string.Format(CultureInfo.InvariantCulture, "clearModalDialog('{0}');alert('Ajax call failed')", dialogDivId),
                        OnSuccess = string.Format(CultureInfo.InvariantCulture, "openModalDialog('{0}', '{1}')", dialogDivId, dialogTitle)
                    };
            

            return ajaxHelper.ActionLink(
                linkText, 
                actionName, 
                controllerName, 
                routeValues,  
                options,
                htmlAttributes
                );
        }

       
    }
}
