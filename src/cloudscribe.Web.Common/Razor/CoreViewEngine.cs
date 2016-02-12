// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2014-10-13
//	Last Modified:              2015-11-18
// 

using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor.Runtime;
using Microsoft.Extensions.OptionsModel;
//using Microsoft.AspNet.Mvc.Razor.OptionDescriptors;
using System.Collections.Generic;

namespace cloudscribe.Web.Common.Razor
{
    /// <summary>
    /// we are subclassing the razor view engine here to be able to customize the
    /// locations and priority where views are searched for
    /// the idea is to keep a set of view under /Views/Sys that are not intended to be modified
    /// and is the lowest priority location that will be searched so that custom versions of the system
    /// views in higher folders will be found first.
    /// ie to customize a view copy it from the /Views/Sys folder into a higher folder or the Views/Shared folder
    /// leaving the original reference view in the /Sys folder
    /// 
    /// TODO: could we detect the skin name from the current request site settings
    /// and then look first in /Views/[SkinName]
    /// ?
    /// Since this is instantiated in Startup it may not be possible to do that
    /// however we might want to look into the Orchard project ThemeAwareViewEngine.cs
    /// they seem to be pulling in views from theme specific folders and not using the normal mvc conventions
    /// I think we should try not to deviate far from normal mvc conventions, ie views should still be below the Views folder
    /// perhaps /Views/themes/[themename]
    /// probably we should use the term theme as opposed to skin
    /// </summary>
    public class CoreViewEngine : RazorViewEngine
    {
        //IRazorViewEngine
        public CoreViewEngine(
            IRazorPageFactory pageFactory, 
            IRazorViewFactory viewFactory,
            IOptions<RazorViewEngineOptions> optionsAccessor,
            IViewLocationCache viewLocationCache):base(pageFactory, viewFactory, optionsAccessor, viewLocationCache)
        {
            //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNet.Mvc.Razor/RazorViewEngine.cs

            // {0} represents the name of the view
            // {1} represents the name of the controller
            // {2} represents the name of the area
            

        }

        private const string ViewExtension = ".cshtml";

        private static readonly IEnumerable<string> _viewLocationFormats = new[]
        {
            "/Views/{1}/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
            "/Views/Sys/{1}/{0}" + ViewExtension,
            "/Views/Sys/Shared/{0}" + ViewExtension,
           
        };

        private static readonly IEnumerable<string> _areaViewLocationFormats = new[]
        {
            "/Areas/{2}/Views/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Views/Shared/{0}" + ViewExtension,
            "/Views/Shared/{0}" + ViewExtension,
            "/Areas/{2}/Views/Sys/{1}/{0}" + ViewExtension,
            "/Areas/{2}/Views/Sys/Shared/{0}" + ViewExtension,
            "/Views/Sys/Shared/{0}" + ViewExtension,
        };

        public override IEnumerable<string> ViewLocationFormats
        {
            get { return _viewLocationFormats; }
        }

        public override IEnumerable<string> AreaViewLocationFormats
        {
            get { return _areaViewLocationFormats; }
        }

    }
}
