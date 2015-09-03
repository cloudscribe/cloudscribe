// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2015-09-03
// 

using System;


namespace cloudscribe.Core.Models
{
    public class CloudscribeCoreVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe-core"; } }

        public Guid ApplicationId { get { return new Guid("b7dcd727-91c3-477f-bc42-d4e5c8721daa"); } }

        public Version GetCodeVersion()
        {
            // this version needs to be maintained in code to set the highest
            // schema script version script that will be run for cloudscribe-core
            // this allows us to work on the next version script without triggering it
            // to execute until we set this version to match the new script version
            return new Version(1, 0, 0, 6);
        }
    }

    //TODO: move this to cms project 
    public class CloudscribeCMsVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe-cms"; } }

        public Guid ApplicationId { get { return new Guid("2ba3e968-dd0b-44cb-9689-188963ed2664"); } }

        public Version GetCodeVersion()
        {
            // this version needs to be maintained in code to set the highest
            // schema script version script that will be run for cloudscribe-core
            // this allows us to work on the next version script without triggering it
            // to execute until we set this version to match the new script version
            return new Version(1, 0, 0, 0);
        }
    }
}
