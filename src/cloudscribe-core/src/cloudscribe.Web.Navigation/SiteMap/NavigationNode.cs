// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-15
// 

using Newtonsoft.Json;
using System.ComponentModel;

namespace cloudscribe.Web.Navigation
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

        /// <summary>
        /// this property can lie, it is not enforced in creating a tree
        /// a node could start out as a root node and then be added as a sub node of another node
        /// not sure we even need this property 
        /// </summary>
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
