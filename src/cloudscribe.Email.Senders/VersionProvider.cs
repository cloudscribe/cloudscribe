using cloudscribe.Versioning;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace cloudscribe.Email.Senders
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(ConfigEmailSenderResolver).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("ro4e2d71-4b27-4c83-93c8-ed4c303209ed"); } }

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