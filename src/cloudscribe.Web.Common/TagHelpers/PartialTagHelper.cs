//// Copyright (c) .NET Foundation. All rights reserved. 
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
////2015-08-05 borrowed from here: https://github.com/aspnet/live.asp.net/blob/dev/src/live.asp.net/TagHelpers/PartialTagHelper.cs

//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Razor.TagHelpers;

//namespace cloudscribe.Web.Common.TagHelpers
//{
//    [HtmlTargetElement("partial", Attributes = "name")]
//    public class PartialTagHelper : TagHelper
//    {
//        private readonly ICompositeViewEngine _viewEngine;

//        public PartialTagHelper(ICompositeViewEngine viewEngine)
//        {
//            _viewEngine = viewEngine;
//        }

//        [ViewContext]
//        public ViewContext ViewContext { get; set; }

//        public string Name { get; set; }

//        public object Model { get; set; }

//        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            output.TagName = null;

//            var partialResult = _viewEngine.FindPartialView(ViewContext, Name);

//            if (partialResult != null && partialResult.Success)
//            {
//                var partialViewData = new ViewDataDictionary(ViewContext.ViewData, Model);
//                var partialWriter = new TagHelperContentWrapperTextWriter(ViewContext.Writer.Encoding, output.Content);
//                var partialViewContext = new ViewContext(ViewContext, partialResult.View, partialViewData, partialWriter);

//                await partialResult.View.RenderAsync(partialViewContext);
//            }
//        }
//    }

//}
