//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2014-10-09
// 

using System;
using System.Configuration.Provider;

namespace cloudscribe.Configuration
{
    public class VersionProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is VersionProvider))
                throw new ArgumentException("The provider parameter must be of type VersionProvider.");

            base.Add(provider);
        }

        new public VersionProvider this[string name]
        {
            get { return (VersionProvider)base[name]; }
        }

        public void CopyTo(VersionProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
