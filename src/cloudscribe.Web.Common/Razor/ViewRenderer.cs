// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-22
// Last Modified:           2018-08-23
// 

//https://github.com/aspnet/Entropy/blob/master/samples/Mvc.RenderViewToString/RazorViewToStringRenderer.cs

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
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
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            //IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider
            )
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
           // _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;

           
        }

        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
       // private readonly IHttpContextAccessor _httpContextAccessor;
        

        public async Task<string> RenderViewAsString<TModel>(string viewName, TModel model)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewName);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            //if (_httpContextAccessor.HttpContext != null)
            //{
            //    return new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());
            //}

            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };

            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }


    }



    //previous implementation

    //public class ViewRenderer
    //{
    //    public ViewRenderer(
    //        IRazorViewEngine viewEngine,
    //        ITempDataProvider tempDataProvider,
    //        IActionContextAccessor actionAccessor,
    //        IServiceProvider serviceProvider
    //        )
    //    {
    //        _viewEngine = viewEngine;
    //        _tempDataProvider = tempDataProvider;
    //        _serviceProvider = serviceProvider;
    //        _actionAccessor = actionAccessor;
    //    }

    //    private IRazorViewEngine _viewEngine;
    //    private ITempDataProvider _tempDataProvider;
    //    private IServiceProvider _serviceProvider;
    //    private IActionContextAccessor _actionAccessor;

    //    public async Task<string> RenderViewAsString<TModel>(string viewName, TModel model)
    //    {
    //        var viewData = new ViewDataDictionary<TModel>(
    //                    metadataProvider: new EmptyModelMetadataProvider(),
    //                    modelState: new ModelStateDictionary())
    //        {
    //            Model = model
    //        };

    //        //var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
    //        //var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    //        //var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);
    //        var actionContext = _actionAccessor.ActionContext;

    //        var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

    //        using (StringWriter output = new StringWriter())
    //        {

    //            ViewEngineResult viewResult = _viewEngine.FindView(actionContext, viewName, true);

    //            ViewContext viewContext = new ViewContext(
    //                actionContext,
    //                viewResult.View,
    //                viewData,
    //                tempData,
    //                output,
    //                new HtmlHelperOptions()
    //            );

    //            await viewResult.View.RenderAsync(viewContext);

    //            return output.GetStringBuilder().ToString();
    //        }
    //    }
    //}
}
