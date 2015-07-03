// Author:					Joe Audette
// Created:					2015-07-02
// Last Modified:			2015-07-03
// 


using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;

namespace cloudscribe.Core.Web.TagHelpers
{
    //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNet.Mvc.TagHelpers/AnchorTagHelper.cs

    /// <summary>
    /// this taghelper detects the bs-modal-link attribute and if found (value doesn't matter)
    /// it decorates the link with the data-ajax- attributes needed to wire up the bootstrap modal
    /// depends on jquery-ajax-unobtrusive and depends on cloudscribe-modaldialog-bootstrap.js
    /// </summary>
    [TargetElement("a", Attributes = BootstrapModalLinkAttributeName)]
    public class BootstrapModalAnchorTagHelper : TagHelper
    {
        private const string BootstrapModalLinkAttributeName = "bs-modal-link";

        public BootstrapModalAnchorTagHelper() : base()
        {
            
        }

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // we don't need to output this attribute it was only used for matching in razor
            output.Attributes.Remove(BootstrapModalLinkAttributeName);
            
            var dialogDivId = Guid.NewGuid().ToString();
            output.Attributes.Add("data-ajax", "true");
            output.Attributes.Add("data-ajax-begin", "prepareModalDialog('" + dialogDivId + "')");
            output.Attributes.Add("data-ajax-failure", "clearModalDialog('" + dialogDivId + "');alert('Ajax call failed')");
            output.Attributes.Add("data-ajax-method", "GET");
            output.Attributes.Add("data-ajax-mode", "replace");
            output.Attributes.Add("data-ajax-success", "openModalDialog('" + dialogDivId + "')");
            output.Attributes.Add("data-ajax-update", "#" + dialogDivId);
            
        }
    }
}
