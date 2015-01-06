// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2015-01-06
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleMemberListViewModel
    {
        public RoleMemberListViewModel()
        {
            Role = new RoleViewModel();
            Members = new List<IUserInfo>();
            Paging = new PagingInfo();
        }

        public string Heading { get; set; }
        public RoleViewModel Role { get; set; }
        public IList<IUserInfo> Members { get; set; }
        public PagingInfo Paging { get; set; }

        private string searchQuery = string.Empty;
        public string SearchQuery
        {
            get { return searchQuery; }
            set { searchQuery = value; }
        }

    }
}
