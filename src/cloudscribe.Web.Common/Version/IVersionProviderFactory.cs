using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Setup
{
    public interface IVersionProviderFactory
    {
        IEnumerable<IVersionProvider> VersionProviders { get; }
        IVersionProvider Get(string name);

    }
}
