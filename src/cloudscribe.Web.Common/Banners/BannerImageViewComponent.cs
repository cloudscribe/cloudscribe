using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common
{
    public class BannerImageViewComponent :ViewComponent
    {
        public BannerImageViewComponent(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        private IBannerService _bannerService;

        public IViewComponentResult Invoke(string viewName = "Default")
        {
            var img = _bannerService.GetImage(HttpContext);
            if (img == null)
            {
                return View("Empty");
            }
            else
            {
                return View(viewName, img);
            }

        }

    }
}
