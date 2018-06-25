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
            _options.FileBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "file" });
            _options.ImageBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "image" });
            _options.DropFileUrl = urlHelper.Action("DropFile", "FileManager");
            _options.CropFileUrl = urlHelper.Action("CropServerImage", "FileManager");


            return Task.FromResult(_options);
        }
    }
}
