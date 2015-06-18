// Author:					Joe Audette
// Created:					2015-06-18
// Last Modified:			2015-06-18
// 

using System.Collections.Generic;

namespace cloudscribe.Configuration
{
    public interface IVersionProviderFactory
    {
        List<IVersionProvider> VersionProviders { get; }
        IVersionProvider Get(string name);

    }
}
