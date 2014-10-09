//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2014-10-09
// 

using System;
using System.Configuration.Provider;


namespace cloudscribe.Configuration
{
    public abstract class VersionProvider : ProviderBase
    {
        public abstract Version GetCodeVersion();
    }
}
