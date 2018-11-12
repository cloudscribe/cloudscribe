using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Versioning
{
    public interface IVersionProviderFactory
    {
        IEnumerable<IVersionProvider> VersionProviders { get; }
        IVersionProvider Get(string name);

    }
}
