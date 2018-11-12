using System;

namespace cloudscribe.Versioning
{
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version CurrentVersion { get; }
    }
}
