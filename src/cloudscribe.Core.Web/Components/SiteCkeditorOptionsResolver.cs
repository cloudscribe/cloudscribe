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
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
            options = ckOptionsAccessor.Value;
        }

        private CkeditorOptions options;
        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccesor;

        public Task<CkeditorOptions> GetCkeditorOptions()
        {
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);

            options.CustomConfigPath = urlHelper.Content(options.CustomConfigPath);
            options.FileBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "file" });
            options.ImageBrowseUrl = urlHelper.Action("FileDialog", "FileManager", new { type = "image" });
            options.DropFileUrl = urlHelper.Action("DropFile", "FileManager");
           

            return Task.FromResult(options);
        }
    }
}
