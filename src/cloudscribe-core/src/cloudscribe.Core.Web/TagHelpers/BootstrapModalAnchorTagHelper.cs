// Author:					Joe Audette
// Created:					2015-07-02
// Last Modified:			2015-07-02
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.TagHelpers;

namespace cloudscribe.Core.Web.TagHelpers
{
    //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNet.Mvc.TagHelpers/AnchorTagHelper.cs

    public class BootstrapModalAnchorTagHelper : AnchorTagHelper
    {
        public BootstrapModalAnchorTagHelper(IHtmlGenerator generator):base(generator)
        {
            
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if(output.Attributes.ContainsName("data-cloudscribe-bootstrapmodal"))
            {
                var dialogDivId = Guid.NewGuid().ToString();
                output.Attributes.Add("data-ajax", "true");
                output.Attributes.Add("data-ajax-begin", "prepareModalDialog(&#39;" + dialogDivId + "&#39;)");
                output.Attributes.Add("data-ajax-failure", "clearModalDialog(&#39;" + dialogDivId + "&#39;);alert(&#39;Ajax call failed&#39;)");
                output.Attributes.Add("data-ajax-method", "GET");
                output.Attributes.Add("data-ajax-mode", "replace");
                output.Attributes.Add("data-ajax-success", "openModalDialog(&#39;" + dialogDivId + "&#39;)");
                output.Attributes.Add("data-ajax-update", "#" + dialogDivId);
         


            }
        }
    }
}
