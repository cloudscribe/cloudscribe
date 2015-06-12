// Author:					Joe Audette
// Created:					2014-12-30
// Last Modified:			2015-06-12
// TODO: implement this a different way without AjaxHelper
// not included in asp.net 5/.net core
// http://stackoverflow.com/questions/28474498/ajaxhelper-mvc6-vnext

//using System;
//using System.Globalization;
//using Microsoft.AspNet.Mvc;
//using Microsoft.AspNet.Mvc.Rendering;


////using System.Web.Mvc;
////using System.Web.Mvc.Ajax;
////using System.Web.Mvc.Html;

//namespace cloudscribe.Core.Web.Helpers
//{
//    public static class ModalDialogHelpers
//    {

//        /// <summary>
//        /// you must include /Scripts/cloudscribe-modaldialog-bootstrap.js
//        /// </summary>
//        /// <param name="ajaxHelper"></param>
//        /// <param name="linkText"></param>
//        /// <param name="actionName"></param>
//        /// <param name="controllerName"></param>
//        /// <param name="routeValues"></param>
//        /// <param name="htmlAttributes"></param>
//        /// <returns></returns>
//        public static HtmlString ModalDialogActionLinkBootstrap(
//            this AjaxHelper ajaxHelper,
//            string linkText,
//            string actionName,
//            string controllerName,
//            object routeValues,
//            object htmlAttributes
//            )
//        {
//            var dialogDivId = Guid.NewGuid().ToString();
//            AjaxOptions options = new AjaxOptions
//            {
//                UpdateTargetId = dialogDivId,
//                InsertionMode = InsertionMode.Replace,
//                HttpMethod = "GET",
//                OnBegin = string.Format(CultureInfo.InvariantCulture, "prepareModalDialog('{0}')", dialogDivId),
//                OnFailure = string.Format(CultureInfo.InvariantCulture, "clearModalDialog('{0}');alert('Ajax call failed')", dialogDivId),
//                OnSuccess = string.Format(CultureInfo.InvariantCulture, "openModalDialog('{0}')", dialogDivId)
//            };


//            return ajaxHelper.ActionLink(
//                linkText,
//                actionName,
//                controllerName,
//                routeValues,
//                options,
//                htmlAttributes
//                );
//        }

//        /// <summary>
//        /// you must include /Scripts/modaldialog-jqui.js
//        /// </summary>
//        /// <param name="ajaxHelper"></param>
//        /// <param name="linkText"></param>
//        /// <param name="dialogTitle"></param>
//        /// <param name="actionName"></param>
//        /// <param name="controllerName"></param>
//        /// <param name="routeValues"></param>
//        /// <param name="htmlAttributes"></param>
//        /// <returns></returns>
//        public static MvcHtmlString ModalDialogActionLinkJQueryUI(
//            this AjaxHelper ajaxHelper,
//            string linkText,
//            string dialogTitle,
//            string actionName,
//            string controllerName,
//            object routeValues,
//            object htmlAttributes
//            )
//        {
//            var dialogDivId = Guid.NewGuid().ToString();
//            AjaxOptions options = new AjaxOptions
//            {
//                UpdateTargetId = dialogDivId,
//                InsertionMode = InsertionMode.Replace,
//                HttpMethod = "GET",
//                OnBegin = string.Format(CultureInfo.InvariantCulture, "prepareModalDialog('{0}')", dialogDivId),
//                OnFailure = string.Format(CultureInfo.InvariantCulture, "clearModalDialog('{0}');alert('Ajax call failed')", dialogDivId),
//                OnSuccess = string.Format(CultureInfo.InvariantCulture, "openModalDialog('{0}', '{1}')", dialogDivId, dialogTitle)
//            };


//            return ajaxHelper.ActionLink(
//                linkText,
//                actionName,
//                controllerName,
//                routeValues,
//                options,
//                htmlAttributes
//                );
//        }


//    }
//}
