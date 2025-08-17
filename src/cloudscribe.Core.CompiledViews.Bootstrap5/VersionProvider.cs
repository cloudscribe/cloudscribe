using cloudscribe.Versioning;
using cloudscribe.Web.Common;
using System;
using System.Reflection;

namespace cloudscribe.Core.CompiledViews.Bootstrap5
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.CompiledViews.Bootstrap5"; } }

        public Guid ApplicationId { get { return new Guid("ro4b2o71-4c27-4o83-93p8-ed4c303209ed"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(CloudscribeCommonResources).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}