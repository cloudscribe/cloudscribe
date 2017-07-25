using cloudscribe.Core.Models;
using cloudscribe.FileManager.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.CoreIntegration
{
    public class MediaPathResolver : IMediaPathResolver
    {
        public MediaPathResolver(
            IHostingEnvironment environment,
            ILogger<MediaPathResolver> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            SiteContext currentSite
            )
        {
            hosting = environment;
            this.currentSite = currentSite;
            options = multiTenantOptionsAccessor.Value;
            log = logger;
        }

        private IHostingEnvironment hosting;
        private SiteContext currentSite;
        private MultiTenantOptions options;
        private ILogger log;

        public Task<MediaRootPathInfo> Resolve(CancellationToken cancellationToken = default(CancellationToken))
        {
            MediaRootPathInfo result = null;
            if (options.UserPerSiteWwwRoot)
            {
                var rootFolderStatus = TryEnsureTenantWwwRoot();
                if(rootFolderStatus.Success)
                {
                    var virtualRoot = GetTenantRootVirtualPath();
                    result = new MediaRootPathInfo(virtualRoot, rootFolderStatus.RootDirectoryPath);
                }

            }
            
            if(result == null) result = GetLegacySiteFolder();
            
            return Task.FromResult(result);

        }

        private MediaRootPathInfo GetLegacySiteFolder()
        {
            var virtualPath = "/" + currentSite.AliasId;
            var fsPath = Path.Combine(hosting.WebRootPath, currentSite.AliasId);
            var result = new MediaRootPathInfo(virtualPath, fsPath);
            return result;
        }

        private string GetTenantRootVirtualPath()
        {
            if(options.Mode == MultiTenantMode.FolderName)
            {
                if(!string.IsNullOrEmpty(currentSite.SiteFolderName))
                {
                    return "/" + currentSite.SiteFolderName;
                }
            }

            return "";
        }

        private EnsureRootFolderResult TryEnsureTenantWwwRoot()
        {
            var result = new EnsureRootFolderResult();

            var siteFilesPath = Path.Combine(hosting.ContentRootPath, options.SiteFilesFolderName);
            if(!Directory.Exists(siteFilesPath))
            {
                try
                {
                    Directory.CreateDirectory(siteFilesPath);
                    result.DidCreateDirectory = true;
                }
                catch(Exception ex)
                {
                    log.LogError($"failed to create folder {siteFilesPath}", ex);
                    result.DidError = true;
                    return result;
                }
            }

            var tenantFolder = Path.Combine(siteFilesPath, currentSite.AliasId);
            if (!Directory.Exists(tenantFolder))
            {
                try
                {
                    Directory.CreateDirectory(tenantFolder);
                    result.DidCreateDirectory = true;
                }
                catch (Exception ex)
                {
                    log.LogError($"failed to create folder {tenantFolder}", ex);
                    result.DidError = true;
                    return result;
                }
            }

            var tenantWwwRoot = Path.Combine(tenantFolder, options.SiteContentFolderName);
            if (!Directory.Exists(tenantWwwRoot))
            {
                try
                {
                    Directory.CreateDirectory(tenantWwwRoot);
                    result.DidCreateDirectory = true;
                }
                catch (Exception ex)
                {
                    log.LogError($"failed to create folder {tenantWwwRoot}", ex);
                    result.DidError = true;
                    return result;
                }
            }

            result.Success = true;
            result.RootDirectoryPath = tenantWwwRoot;

            return result;
        }

        


    }

    class EnsureRootFolderResult
    {
        public bool Success { get; set; }
        public bool DidCreateDirectory { get; set; }

        public string RootDirectoryPath { get; set; }
        public bool DidError { get; set; }

    }
}
