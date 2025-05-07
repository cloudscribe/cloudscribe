using cloudscribe.Versioning;
using cloudscribe.Web.Common;
using System;
using System.Reflection;

namespace cloudscribe.IdentityServerIntegration.CompiledViews.Bootstrap5
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.IdentityServerIntegration.CompiledViews.Bootstrap5"; } }

        public Guid ApplicationId { get { return new Guid("ab4b2o71-4c27-4o83-93p8-ed4c209209ed"); } }

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