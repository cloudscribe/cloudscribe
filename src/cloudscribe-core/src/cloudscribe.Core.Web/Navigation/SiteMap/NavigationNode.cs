// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-13
// 

using System.ComponentModel;
using Newtonsoft.Json;
using System;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationNode : INavigationNode, INavigationNodeLocalization, INavigationNodeRenderSettings
    {
        #region INavigationNode

        [JsonRequired]
        public string Key { get; set; } = string.Empty;

        [DefaultValue("")]
        public string ParentKey { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Text { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Title { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Url { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Controller { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Action { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsRootNode { get; set; } = false;

        #endregion

        #region INavigationNodeLocalization

        [DefaultValue("")]
        public string ResourceName { get; set; } = string.Empty;

        [DefaultValue("")]
        public string ResourceTextKey { get; set; } = string.Empty;

        [DefaultValue("")]
        public string ResourceTitleKey { get; set; } = string.Empty;

        #endregion

        #region INavigationNodeRenderSettings

        [DefaultValue(true)]
        public bool IncludeAmbientValuesInUrl { get; set; } = true;

        [DefaultValue("")]
        public string PreservedRouteParameters { get; set; } = string.Empty;

        [DefaultValue("")]
        public string ComponentVisibility { get; set; } = string.Empty;

        [DefaultValue("")]
        public string ViewRoles { get; set; } = string.Empty;

        #endregion

    }
}
