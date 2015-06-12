// Author:					Joe Audette
// Created:					2015-06-10
// Last Modified:			2015-06-10
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

        private int siteId = -1;

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        private string keyName = string.Empty;

        public string KeyName
        {
            get { return keyName; }
            set { keyName = value; }
        }

        private string keyValue = string.Empty;

        public string KeyValue
        {
            get { return keyValue; }
            set { keyValue = value; }
        }

        private string groupName = string.Empty;

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        private bool isDirty = false;

        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }
    }
}
