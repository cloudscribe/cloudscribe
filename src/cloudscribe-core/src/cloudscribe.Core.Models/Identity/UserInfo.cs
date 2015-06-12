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
    //[Serializable]
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

        private bool isDeleted = false;

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        //2010-12-19 changed public name from ApprovedForForums
        // the property was previously intended for forums but was never used there
        // the field name in the db will remain the same but the purpose of this property 
        // is now for requiring approval of newly registered users before they can login
        // if that is required by siteSettings

        private bool approvedForForums = true;
        public bool ApprovedForLogin
        {
            get { return approvedForForums; }
            set { approvedForForums = value; }
        }

        private string avatarUrl = string.Empty;

        public string AvatarUrl
        {
            get { return avatarUrl; }
            set { avatarUrl = value; }
        }

        private DateTime dateOfBirth = DateTime.MinValue;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        private DateTime dateCreated = DateTime.UtcNow;

        public DateTime CreatedUtc
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        private bool displayInMemberList = true;

        public bool DisplayInMemberList
        {
            get { return displayInMemberList; }
            set { displayInMemberList = value; }
        }

        private string gender = string.Empty;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private bool isLockedOut = false;

        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        private string country = string.Empty;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        private string state = string.Empty;

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        private DateTime lastActivityDate = DateTime.MinValue;

        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        private DateTime lastLoginDate = DateTime.MinValue;

        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }

        private string phoneNumber = string.Empty;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private bool phoneNumberConfirmed = false;

        public bool PhoneNumberConfirmed
        {
            get { return phoneNumberConfirmed; }
            set { phoneNumberConfirmed = value; }
        }

        private bool profileApproved = true;

        public bool ProfileApproved
        {
            get { return profileApproved; }
            set { profileApproved = value; }
        }

        private string timeZoneId = "Eastern Standard Time"; //default

        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        private int totalPosts = 0;

        public int TotalPosts
        {
            get { return totalPosts; }
            set { totalPosts = value; }
        }

        private decimal totalRevenue = 0;

        public decimal TotalRevenue
        {
            get { return totalRevenue; }
            set { totalRevenue = value; }
        }

        private bool trusted = false;

        public bool Trusted
        {
            get { return trusted; }
            set { trusted = value; }
        }

        private string webSiteUrl = string.Empty;

        public string WebSiteUrl
        {
            get { return webSiteUrl; }
            set { webSiteUrl = value; }
        }
    }
}
