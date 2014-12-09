// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2014-12-08
//

using cloudscribe.Core.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {

            UserList = new List<IUserInfo>();
            Paging = new PagingInfo();
        }

        public string Heading { get; set; }
        public IList<IUserInfo> UserList { get; set; }
        public PagingInfo Paging { get; set; }

    }
}
