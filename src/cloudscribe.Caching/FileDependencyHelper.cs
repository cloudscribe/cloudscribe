using log4net;
using System;
using System.IO;
using System.Web.Hosting;

namespace cloudscribe.Caching
{
    public static class FileDependencyHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileDependencyHelper));

        public static string GetPathToCacheDependencyFile(string cacheKey)
        {
            EnsureDirectory(HostingEnvironment.MapPath("~/Data/systemfiles/"));

            return HostingEnvironment.MapPath(
                "~/Data/systemfiles/" + cacheKey + "_cachedependency.config");
        }

        private static void EnsureDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) return;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void EnsureCacheFile(string pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            if (!File.Exists(pathToCacheFile))
            {
                TouchCacheFile(pathToCacheFile);
            }
        }

        public static void TouchCacheFile(String pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            try
            {
                if (File.Exists(pathToCacheFile))
                {
                    File.SetLastWriteTimeUtc(pathToCacheFile, DateTime.UtcNow);
                }
                else
                {
                    File.CreateText(pathToCacheFile).Close();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error(ex);
            }
        }


    }
}
