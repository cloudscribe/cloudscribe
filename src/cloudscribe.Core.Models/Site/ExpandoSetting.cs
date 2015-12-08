// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-10
// Last Modified:			2015-12-08
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

        private string keyName = string.Empty;
        public string KeyName
        {
            get { return keyName ?? string.Empty; }
            set { keyName = value; }
        }

        private string keyValue = string.Empty;
        public string KeyValue
        {
            get { return keyValue ?? string.Empty; }
            set { keyValue = value; }
        }

        private string groupName = string.Empty;
        public string GroupName
        {
            get { return groupName ?? string.Empty; }
            set { groupName = value; }
        }

        
        public bool IsDirty { get; set; } = false;
       
    }
}
