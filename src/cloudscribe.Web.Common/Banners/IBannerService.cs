using Microsoft.AspNetCore.Http;

namespace cloudscribe.Web.Common
{
    public interface IBannerService
    {
        BannerImage GetImage(HttpContext context);
    }
}
