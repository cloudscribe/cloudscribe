using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudscribe.Web.Common
{
    public class ConfigBannerService : IBannerService
    {
        public ConfigBannerService(IOptions<BannerImageMap> imageMapAccessor)
        {
            _imageMap = imageMapAccessor.Value;
        }

        private BannerImageMap _imageMap;

        public BannerImage GetImage(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;
            foreach (var img in _imageMap.Images)
            {
                if (img.PageUrlPaths.Contains(requestPath)) { return img; }
            }

            return null;
        }
    }
}
