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
            
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);

            _options.CustomConfigPath = urlHelper.Content(_options.CustomConfigPath);

            if(string.IsNullOrWhiteSpace(_options.FileBrowseUrl)) _options.FileBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "file" });
            else _options.FileBrowseUrl = urlHelper.Content(_options.FileBrowseUrl);

            if(string.IsNullOrWhiteSpace( _options.ImageBrowseUrl)) _options.ImageBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "image" });
            else _options.ImageBrowseUrl= urlHelper.Content(_options.ImageBrowseUrl);

            if(string.IsNullOrWhiteSpace(_options.VideoBrowseUrl)) _options.VideoBrowseUrl  = urlHelper.Action("FileDialog", "FileManager", new { type = "video" });
            else _options.VideoBrowseUrl = urlHelper.Content(_options.VideoBrowseUrl);

            if(string.IsNullOrWhiteSpace(_options.AudioBrowseUrl)) _options.AudioBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "audio" });
            else _options.AudioBrowseUrl = urlHelper.Content(_options.AudioBrowseUrl);

            if(string.IsNullOrWhiteSpace(_options.DropFileUrl)) _options.DropFileUrl = urlHelper.Action("DropFile", "FileManager");
            else _options.DropFileUrl = urlHelper.Content(_options.DropFileUrl);

            if(string.IsNullOrWhiteSpace(_options.CropFileUrl)) _options.CropFileUrl = urlHelper.Action("CropServerImage", "FileManager");
            else _options.CropFileUrl = urlHelper.Content(_options.CropFileUrl);

            return Task.FromResult(_options);
        }
    }
}
