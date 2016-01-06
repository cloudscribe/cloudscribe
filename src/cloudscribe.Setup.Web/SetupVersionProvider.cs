// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-03
// Last Modified:			2016-01-06
// 

using cloudscribe.Core.Models.Setup;
using System;

namespace cloudscribe.Setup.Web
{
    /// <summary>
    /// this declares the version and applicationid for the cloudscribe setup system itself
    /// </summary>
    public class SetupVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe-setup"; } }

        public Guid ApplicationId { get { return new Guid("9e1f3fc4-e46a-46ed-bc6b-a08c649fd4c0"); } }

        public Version CurrentVersion
        {
            // this version needs to be maintained in code to set the highest
            // schema script version script that will be run for cloudscribe-core
            // this allows us to work on the next version script without triggering it
            // to execute until we set this version to match the new script version
            get { return new Version(1, 0, 0, 0); }
        }
    }
}
