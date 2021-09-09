// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-06-09
// Last Modified:           2017-06-09
// 

using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteCkeditorOptionsResolver : ICkeditorOptionsResolver
    {
        public SiteCkeditorOptionsResolver(
            IOptions<CkeditorOptions> ckOptionsAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor
            )
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _options = ckOptionsAccessor.Value;
        }

        private CkeditorOptions _options;
        private IUrlHelperFactory _urlHelperFactory;
        private IActionContextAccessor _actionContextAccesor;

        public Task<CkeditorOptions> GetCkeditorOptions()
        {
            // jk - We need to construct a new instance here on each invocation, since the existing _options is a singleton
            // and so persists across invocations - creating a multi-tenancy bug whereby the values are only ever correct 
            // for the first accessed tenant. #766

            var result = new CkeditorOptions();

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);

            result.CustomConfigPath = urlHelper.Content(_options.CustomConfigPath);

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
