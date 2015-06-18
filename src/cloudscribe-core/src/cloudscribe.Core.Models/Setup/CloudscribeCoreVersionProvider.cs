//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2015-04-20
// 

using System;
using cloudscribe.Configuration;

namespace cloudscribe.Core.Models
{
    public class CloudscribeCoreVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe-core"; } }

        public Version GetCodeVersion()
        {
            // this version needs to be maintained in code to set the highest
            // schema script version script that will be run for cloudscribe-core
            // this allows us to work on the next version script without triggering it
            // to execute until we set this version to match the new script version
            return new Version(1, 0, 0, 4);
        }
    }
}
