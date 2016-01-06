// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-06
// Last Modified:			2016-01-06
// 

using cloudscribe.Core.Models.Setup;
using System;


namespace cloudscribe.Setup.Web
{
    // we can build a list of these from the database mp_SchemaVersion table
    // to show a list of installed schemas
    public class VersionItem : IVersionProvider
    {
        public string Name { get; set; } = string.Empty;
        public Guid ApplicationId { get; set; } = Guid.Empty;
        public Version CurrentVersion { get; set; } 
    }
}
