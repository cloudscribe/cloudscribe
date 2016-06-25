// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-22
// Last Modified:           2016-06-25
// 

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Razor
{
    /// <summary>
    /// the goal of this class is to provide an easy way to produce an html string using 
    /// Razor templates and models, for use in generating html email.
    /// </summary>
    public class ViewRenderer
    {
        public ViewRenderer(
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IActionContextAccessor actionAccessor
            )
        {
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
            this.actionAccessor = actionAccessor;
            
        }

        private ICompositeViewEngine viewEngine;
        private ITempDataProvider tempDataProvider;
        private IActionContextAccessor actionAccessor;
        
        public async Task<string> RenderViewAsString<TModel>(string viewName, TModel model)
        {

            var viewData = new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
            {
                Model = model
            };

            var actionContext = actionAccessor.ActionContext;
            
            var tempData = new TempDataDictionary(actionContext.HttpContext, tempDataProvider);

            using (StringWriter output = new StringWriter())
            {
              
                ViewEngineResult viewResult = viewEngine.FindView(actionContext, viewName, true);

                ViewContext viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewData,
                    tempData,
                    output,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return output.GetStringBuilder().ToString();
            }
        }
    }
}
