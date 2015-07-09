// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-09
// 


namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationNode : INavigationNode, INavigationNodeLocalization, INavigationNodeRenderSettings
    {
        #region INavigationNode

        public string Key { get; set; } = string.Empty;
        public string ParentKey { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool IsRootNode { get; set; } = false;

        #endregion

        #region INavigationNodeLocalization

        public string ResourceName { get; set; } = string.Empty;
        public string ResourceTextKey { get; set; } = string.Empty;
        public string ResourceTitleKey { get; set; } = string.Empty;

        #endregion

        #region INavigationNodeRenderSettings

        public bool IncludeAmbientValuesInUrl { get; set; } = true;
        public string PreservedRouteParameters { get; set; } = string.Empty;
        public string ComponentVisibility { get; set; } = string.Empty;
        public string ViewRoles { get; set; } = string.Empty;

        #endregion

    }
}
