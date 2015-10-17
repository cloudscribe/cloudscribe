// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-19
// Last Modified:			2015-10-17
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;
using System;

namespace cloudscribe.Core.Web.Components
{
    public class RequestSiteResolver : ISiteResolver
    {
        public RequestSiteResolver(
            ISiteRepository siteRepository,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IHttpContextAccessor httpContextAccessor)
        {
            contextAccessor = httpContextAccessor;
            siteRepo = siteRepository;
            this.multiTenantOptions = multiTenantOptions.Value;

        }

        private MultiTenantOptions multiTenantOptions;
        private IHttpContextAccessor contextAccessor;
        private ISiteRepository siteRepo;
        private string requestPath;
        private string host;

        public ISiteSettings Resolve()
        {
            return Resolve(contextAccessor.HttpContext);
        }

        public ISiteSettings Resolve(HttpContext context)
        {
            requestPath = context.Request.Path.Value;
            // this seems to behave different in IIS vs weblistener
            string pathBase = context.Request.PathBase;
            // this is true for folder sites
            if (pathBase.Length > 0)
            {
                if(!requestPath.StartsWith(pathBase))
                {
                    requestPath = pathBase + requestPath;
                }
                
            }
            host = context.Request.Host.Value;
            int siteId = -1;
           
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                string siteFolderName = GetFirstFolderSegment(requestPath);
                if (siteFolderName.Length == 0) siteFolderName = "root";
                siteId = siteRepo.GetSiteIdByFolderNonAsync(siteFolderName);

                if (siteId == -1)
                {
                    // error would be expected here on initial setup
                    // when the db has not been set up and no site exists
                    throw new InvalidOperationException("could not resolve site id");
                }
                else
                {
                    return siteRepo.FetchNonAsync(siteId);
                }
            }
            else
            {
                return siteRepo.FetchNonAsync(host);
            }


        }

        public static string GetFirstFolderSegment(string url)
        {

            // find first level folder name
            // after site root
            string folderName = string.Empty;

            string requestPath = url.Replace("https://", string.Empty).Replace("http://", string.Empty);
            
            if (requestPath == "/") return folderName;

            //  cloudscribe/Content/css?v=vSkk4S2yX-JkF2jnutJR9WADNnO1X7e9w005ClDaRCs1

            int indexOfFirstSlash = requestPath.IndexOf("/");
            int indexOfLastSlash = requestPath.LastIndexOf("/");

            if (
                (indexOfFirstSlash > -1)
                && (indexOfLastSlash > (indexOfFirstSlash + 1))
                )
            {
                requestPath = requestPath.Substring(indexOfFirstSlash + 1, requestPath.Length - indexOfFirstSlash - 1);

                if (requestPath.IndexOf("/") > -1)
                {
                    folderName = requestPath.Substring(0, requestPath.IndexOf("/"));

                }
            }

           
            //    /en
            if ((0 == indexOfFirstSlash) && (0 == indexOfLastSlash) && (requestPath.Length > 1))
            {
                folderName = requestPath.Substring(1);
            }

            return folderName;
        }

    }
}
