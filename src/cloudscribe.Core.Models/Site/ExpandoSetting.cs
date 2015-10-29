// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-10
// Last Modified:			2015-08-05
// 



namespace cloudscribe.Core.Models
{
    /// <summary>
    /// previously we were using a DataTable but DataTable is not supported in .net core
    /// so changed to a list of ExpandoSettings
    /// </summary>
    public class ExpandoSetting
    {
        public ExpandoSetting()
        { }
        
        public int SiteId { get; set; } = -1;
        public string KeyName { get; set; } = string.Empty; 
        public string KeyValue { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public bool IsDirty { get; set; } = false;
       
    }
}
