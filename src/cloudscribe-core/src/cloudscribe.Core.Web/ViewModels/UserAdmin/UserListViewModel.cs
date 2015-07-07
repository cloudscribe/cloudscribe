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
            Paging = new PaginationSettings();
        }

        public string Heading { get; set; }
        public IList<IUserInfo> UserList { get; set; }
        public PaginationSettings Paging { get; set; }

        private string alphaQuery = string.Empty;
        public string AlphaQuery
        {
            get { return alphaQuery; }
            set { alphaQuery = value; }
        }

        private string searchQuery = string.Empty;
        public string SearchQuery
        {
            get { return searchQuery; }
            set { searchQuery = value; }
        }

        private string ipQuery = string.Empty;
        public string IpQuery
        {
            get { return ipQuery; }
            set { ipQuery = value; }
        }

        private bool showAlphaPager = true;

        public bool ShowAlphaPager
        {
            get { return showAlphaPager; }
            set { showAlphaPager = value; }
        }

    }
}
