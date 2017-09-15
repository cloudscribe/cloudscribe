using System.Collections.Generic;

namespace cloudscribe.Web.Common
{
    public class BannerImageMap
    {
        public BannerImageMap()
        {
            Images = new List<BannerImage>();
        }

        public List<BannerImage> Images { get; set; }
    }
}
