using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteSummernoteOptionsResolver: ISummernoteOptionsResolver
    {
        public SiteSummernoteOptionsResolver(
            IOptions<SummernoteOptions> summernoteOptionsAccessor,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _options = summernoteOptionsAccessor.Value;
            _options = summernoteOptionsAccessor.Value;
        }

        private SummernoteOptions _options;
        private IUrlHelperFactory _urlHelperFactory;
        private IHttpContextAccessor _httpContextAccessor;

        public Task<SummernoteOptions> GetSummernoteOptions()
        {
            // jk - We need to construct a new instance here on each invocation, since the existing _options is a singleton
            // and so persists across invocations - creating a multi-tenancy bug whereby the values are only ever correct 
            // for the first accessed tenant. #766

            var result = new SummernoteOptions();

            // Use IHttpContextAccessor instead of deprecated IActionContextAccessor
            var actionContext = new ActionContext(
                _httpContextAccessor.HttpContext,
                _httpContextAccessor.HttpContext.GetRouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            result.CustomConfigPath = urlHelper.Content(_options.CustomConfigPath);
            result.CustomToolbarConfigPath = urlHelper.Content(_options.CustomToolbarConfigPath);

            if (string.IsNullOrWhiteSpace(_options.FileBrowseUrl)) result.FileBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "file" });
            else result.FileBrowseUrl = urlHelper.Content(_options.FileBrowseUrl);

            if (string.IsNullOrWhiteSpace(_options.ImageBrowseUrl)) result.ImageBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "image" });
            else result.ImageBrowseUrl = urlHelper.Content(_options.ImageBrowseUrl);

            if (string.IsNullOrWhiteSpace(_options.VideoBrowseUrl)) result.VideoBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "video" });
            else result.VideoBrowseUrl = urlHelper.Content(_options.VideoBrowseUrl);

            if (string.IsNullOrWhiteSpace(_options.AudioBrowseUrl)) result.AudioBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "audio" });
            else result.AudioBrowseUrl = urlHelper.Content(_options.AudioBrowseUrl);

            if (string.IsNullOrWhiteSpace(_options.DropFileUrl)) result.DropFileUrl = urlHelper.Action("DropFile", "FileManager");
            else result.DropFileUrl = urlHelper.Content(_options.DropFileUrl);

            if (string.IsNullOrWhiteSpace(_options.CropFileUrl)) result.CropFileUrl = urlHelper.Action("CropServerImage", "FileManager");
            else result.CropFileUrl = urlHelper.Content(_options.CropFileUrl);

            return Task.FromResult(result);
        }
    }
}
