using cloudscribe.Web.SiteMap;
using cloudscribe.Web.SiteMap.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// Implemented to solve this: https://github.com/cloudscribe/cloudscribe.Web.Navigation/issues/46
    /// </summary>
    public class FolderSiteMapController : SiteMapController
    {
        public FolderSiteMapController(
            ILogger<SiteMapController> logger,
            IEnumerable<ISiteMapNodeService> nodeProviders = null
            ) : base(logger, nodeProviders)
        {
            _log = logger;
            _nodeProviders = nodeProviders;
        }

        private ILogger _log;
        private IEnumerable<ISiteMapNodeService> _nodeProviders;

        [HttpGet]
        [ResponseCache(CacheProfileName = "SiteMapCacheProfile")]
        //[Route("{folder:sitefolder}/api/sitemap")]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }
    }
}
