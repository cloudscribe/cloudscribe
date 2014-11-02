//  Author:                     Joe Audette
//  Created:                    2014-10-13
//	Last Modified:              2014-10-17
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace cloudscribe.Core.Web
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
        public CoreViewEngine()
        {
            // {0} represents the name of the view
            // {1} represents the name of the controller
            // {2} represents the name of the area

            ViewLocationFormats = new string[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Sys/{1}/{0}.cshtml",
                "~/Views/Sys/Shared/{0}.cshtml"
            };

            MasterLocationFormats = new string[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Sys/{1}/{0}.cshtml",
                "~/ViewsSys/Shared/{0}.cshtml"
            };

            PartialViewLocationFormats = new string[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Sys/{1}/{0}.cshtml",
                "~/Views/Sys/Shared/{0}.cshtml"
            };



            AreaViewLocationFormats = new string[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/Shared/{0}.cshtml"
            };

            AreaMasterLocationFormats = new string[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/Shared/{0}.cshtml"
            };

            AreaPartialViewLocationFormats = new string[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Sys/Shared/{0}.cshtml"
            };

            

            
        }

    }
}
