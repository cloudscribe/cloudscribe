using System;

namespace cloudscribe.Web.Common.Setup
{
    [Obsolete("this will be removed from cloiudscribe.Web.Common in a future release, please use the version in the new package cloudscribe.Versioning")]
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version CurrentVersion { get; }
    }
}
