// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-22
// Last Modified:           2016-05-18
// 

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
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
            IHttpContextAccessor contextAccesor)
        {
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
            this.contextAccesor = contextAccesor;
        }

        private ICompositeViewEngine viewEngine;
        private ITempDataProvider tempDataProvider;
        private IHttpContextAccessor contextAccesor;

        public async Task<string> RenderViewAsString<TModel>(string viewName, TModel model)
        {

            var viewData = new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
            {
                Model = model
            };

            var actionContext = new ActionContext(contextAccesor.HttpContext, new RouteData(), new ActionDescriptor());
            var tempData = new TempDataDictionary(contextAccesor.HttpContext, tempDataProvider);
            
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
