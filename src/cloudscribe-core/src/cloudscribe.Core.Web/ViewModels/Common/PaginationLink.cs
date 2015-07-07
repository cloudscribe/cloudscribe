// Author:					Joe Audette
// Created:					2015-07-06
// Last Modified:			2015-07-06
//

namespace cloudscribe.Core.Web.ViewModels
{
    public class PaginationLink
    {
        public bool Active { get; set; } = false;

        public bool IsCurrent { get; set; } = false;

        public int? PageIndex { get; set; } = -1;

        public string DisplayText { get; set; } = string.Empty;

        public string DisplayTitle { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public bool IsSpacer { get; set; } = false;
    }
}
