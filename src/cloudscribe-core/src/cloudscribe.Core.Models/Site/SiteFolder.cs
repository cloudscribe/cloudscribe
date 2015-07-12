// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2014-08-17
// 

using System;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// represents the first folder segment in an url after a root level web site
    /// used for resolving the siteid based on folder name
    /// for multi-site/tennant based on the first folder segment of an url
    /// </summary>
    public interface ISiteFolder
    {
        Guid Guid { get; set; }
        Guid SiteGuid { get; set; }
        string FolderName { get; set; }

    }

    public class SiteFolder : ISiteFolder
    {
        public Guid Guid { get; set; }
        public Guid SiteGuid { get; set; }
        public string FolderName { get; set; }
    }
}
