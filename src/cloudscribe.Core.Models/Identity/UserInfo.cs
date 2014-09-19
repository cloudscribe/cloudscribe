using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// lighter weight version of user for lists and dropdowns
    /// base class for SiteUser
    /// </summary>
    [Serializable]
    public class UserInfo : IUserInfo
    {
        public UserInfo()
        { }

        private int userId = -1;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private Guid userGuid = Guid.Empty;

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        private int siteId = -1;

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        private string email = string.Empty;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string loginName = string.Empty;

        public string UserName
        {
            get { return loginName; }
            set { loginName = value; }
        }

        private string name = string.Empty;

        public string DisplayName
        {
            get { return name; }
            set { name = value; }
        }

        private string firstName = string.Empty;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }


        private string lastName = string.Empty;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

    }
}
