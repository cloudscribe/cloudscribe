using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

namespace cloudscribe.Core.Web.Helpers
{
    public static class ModalDialogHelpers
    {
        sealed class DialogActionResult : ActionResult
        {
            public DialogActionResult(string message)
            {
                Message = message ?? string.Empty;
            }

            string Message { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.Write(string.Format("<div data-dialog-close='true' data-dialog-result='{0}' />", Message));
            }
        }

        public static MvcHtmlString ModalDialogActionLink(
            this AjaxHelper ajaxHelper, 
            string linkText,  
            string dialogTitle,
            string actionName)
        {
            var dialogDivId = Guid.NewGuid().ToString();
            return ajaxHelper.ActionLink(linkText, actionName, routeValues: null,
                    ajaxOptions: new AjaxOptions
                    {
                        UpdateTargetId = dialogDivId,
                        InsertionMode = InsertionMode.Replace,
                        HttpMethod = "GET",
                        OnBegin = string.Format(CultureInfo.InvariantCulture, "prepareModalDialog('{0}')", dialogDivId),
                        OnFailure = string.Format(CultureInfo.InvariantCulture, "clearModalDialog('{0}');alert('Ajax call failed')", dialogDivId),
                        OnSuccess = string.Format(CultureInfo.InvariantCulture, "openModalDialog('{0}', '{1}')", dialogDivId, dialogTitle)
                    });
        }

        public static MvcHtmlString ModalDialogActionLink(
            this AjaxHelper ajaxHelper, 
            string linkText, 
            string dialogTitle,
            string actionName, 
            string controllerName, 
            object routeValues,
            object htmlAttributes,
            string targetId
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

        public static MvcForm BeginModalDialogForm(
            this AjaxHelper ajaxHelper,
            string actionName,
            string controllerName,
            object routeValues)
        {
            AjaxOptions options = new AjaxOptions
            {
                HttpMethod = "POST"
            };

            return ajaxHelper.BeginForm(actionName, controllerName, routeValues, options);
        }

        public static ActionResult DialogResult(this Controller controller)
        {
            return DialogResult(controller, string.Empty);
        }

        public static ActionResult DialogResult(this Controller controller, string message)
        {
            return new DialogActionResult(message);
        }
    }
}
