//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2014-08-17
//// Last Modified:			2015-12-09
//// 

//using System;

//namespace cloudscribe.Core.Models
//{
//    /// <summary>
//    /// represents the first folder segment in an url after a root level web site
//    /// used for resolving the siteid based on folder name
//    /// for multi-site/tennant based on the first folder segment of an url
//    /// </summary>
//    public interface ISiteFolder
//    {
//        Guid Guid { get; set; }
//        Guid SiteGuid { get; set; }
//        string FolderName { get; set; }

//    }

//    public class SiteFolder : ISiteFolder
//    {
//        public Guid Guid { get; set; } = Guid.Empty;
//        public Guid SiteGuid { get; set; } = Guid.Empty;

//        private string folderName = string.Empty;
//        public string FolderName
//        {
//            get { return folderName ?? string.Empty; }
//            set { folderName = value; }
//        }

//        public static SiteFolder FromISiteFolder(ISiteFolder i)
//        {
//            SiteFolder f = new SiteFolder();
//            f.FolderName = i.FolderName;
//            f.Guid = i.Guid;
//            f.SiteGuid = i.SiteGuid;

//            return f;
//        }


//    }
//}
