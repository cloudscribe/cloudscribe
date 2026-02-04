using System.Reflection;
using cloudscribe.Versioning;

namespace cloudscribe.QueryTool.Models
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(SavedQuery).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("da4e2d71-4b27-4c83-93c8-cc4c209209ed"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}