// Author:					Joe Audette
// Created:					2015-05-05
// Last Modified:			2015-05-05
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteHostMappingsViewModel
    {
        public SiteHostMappingsViewModel()
        {
            HostMappings = new List<ISiteHost>();
        }

        public string Heading { get; set; }
        public IList<ISiteHost> HostMappings { get; set; }

        private int siteListReturnPageNumber = -1;

        public int SiteListReturnPageNumber
        {
            get { return siteListReturnPageNumber; }
            set { siteListReturnPageNumber = value; }
        }

        //private string newHostName = string.Empty;
    }
}
