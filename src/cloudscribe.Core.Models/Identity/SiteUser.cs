// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2014-08-20
// 

using Microsoft.AspNet.Identity;
using System;

namespace cloudscribe.Core.Models
{
    
    [Serializable]
    public class SiteUser : UserInfo, IUser, ISiteUser
    {
        public SiteUser()
        { }

    
        public string Id
        {
            get { return UserGuid.ToString(); }
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

        private string authorBio = string.Empty;

        public string AuthorBio
        {
            get { return authorBio; }
            set { authorBio = value; }
        }

        private string avatarUrl = string.Empty;

        public string AvatarUrl
        {
            get { return avatarUrl; }
            set { avatarUrl = value; }
        }

        private string comment = string.Empty;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private DateTime dateOfBirth = DateTime.MinValue;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        private DateTime dateCreated = DateTime.UtcNow;
        //TODO rename to CreatedUtc after db is wired up to map it
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

        private string editorPreference = string.Empty; // use site default

        public string EditorPreference
        {
            get { return editorPreference; }
            set { editorPreference = value; }
        }

        

        private string loweredEmail = string.Empty;

        public string LoweredEmail
        {
            get { return loweredEmail; }
            set { loweredEmail = value; }
        }

        private bool emailConfirmed = false;
        public bool EmailConfirmed
        {
            get { return emailConfirmed; }
            set { emailConfirmed = value; }
        }

        private Guid emailChangeGuid = Guid.Empty;

        public Guid EmailChangeGuid
        {
            get { return emailChangeGuid; }
            set { emailChangeGuid = value; }
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

        

        private DateTime? lockoutEndDateUtc = null;

        public DateTime? LockoutEndDateUtc
        {
            get { return lockoutEndDateUtc; }
            set { lockoutEndDateUtc = value; }
        }

        

        private string newEmail = string.Empty;

        public string NewEmail
        {
            get { return newEmail; }
            set { newEmail = value; }
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

        private DateTime lastPasswordChangedDate = DateTime.MinValue;

        public DateTime LastPasswordChangedDate
        {
            get { return lastPasswordChangedDate; }
            set { lastPasswordChangedDate = value; }
        }

        private DateTime lastLockoutDate = DateTime.MinValue;

        public DateTime LastLockoutDate
        {
            get { return lastLockoutDate; }
            set { lastLockoutDate = value; }
        }

        private string mobilePIN = string.Empty;

        public string MobilePin
        {
            get { return mobilePIN; }
            set { mobilePIN = value; }
        }

        private string openIDURI = string.Empty;

        public string OpenIdUri
        {
            get { return openIDURI; }
            set { openIDURI = value; }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private bool mustChangePwd = false;

        public bool MustChangePwd
        {
            get { return mustChangePwd; }
            set { mustChangePwd = value; }
        }

        private string passwordHash = string.Empty; // used for asp.net identity pwd field is still used for membership

        /// <summary>
        /// since aspnet identity doesn't directly support salt
        /// we have to store the hash, the salt and the format in a single field
        /// so our custom hasher will have it
        /// http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
        /// https://aspnetidentity.codeplex.com/workitem/2333
        /// </summary>
        public string PasswordHash
        {
            get
            {
                if (passwordHash.Length == 0)
                {
                    return password + "|" + passwordSalt + "|" + passwordFormat.ToString();
                }
                return passwordHash;
            }
            set { passwordHash = value; }
        }

        private int passwordFormat = 0; //plain text 1= hashed, 2 = encrypted

        public int PasswordFormat
        {
            get { return passwordFormat; }
            set { passwordFormat = value; }
        }

        private string passwordQuestion = string.Empty;

        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        private string passwordAnswer = string.Empty;

        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        private int failedPasswordAttemptCount = 0;

        public int FailedPasswordAttemptCount
        {
            get { return failedPasswordAttemptCount; }
            set { failedPasswordAttemptCount = value; }
        }

        private DateTime failedPasswordAttemptWindowStart = DateTime.MinValue;

        public DateTime FailedPasswordAttemptWindowStart
        {
            get { return failedPasswordAttemptWindowStart; }
            set { failedPasswordAttemptWindowStart = value; }
        }

        private int failedPasswordAnswerAttemptCount;

        public int FailedPasswordAnswerAttemptCount
        {
            get { return failedPasswordAnswerAttemptCount; }
            set { failedPasswordAnswerAttemptCount = value; }
        }

        private DateTime failedPasswordAnswerAttemptWindowStart = DateTime.MinValue;

        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get { return failedPasswordAnswerAttemptWindowStart; }
            set { failedPasswordAnswerAttemptWindowStart = value; }
        }

        

        private Guid passwordResetGuid = Guid.Empty;

        public Guid PasswordResetGuid
        {
            get { return passwordResetGuid; }
            set { passwordResetGuid = value; }
        }

        private string passwordSalt = string.Empty;

        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
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

        private Guid registerConfirmGuid = Guid.Empty;

        public Guid RegisterConfirmGuid
        {
            get { return registerConfirmGuid; }
            set { registerConfirmGuid = value; }
        }

        private bool rolesChanged = false;

        public bool RolesChanged
        {
            get { return rolesChanged; }
            set { rolesChanged = value; }
        }

        private string securityStamp = string.Empty;

        public string SecurityStamp
        {
            get { return securityStamp; }
            set { securityStamp = value; }
        }

        private string signature = string.Empty;

        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        private string skin = string.Empty;

        public string Skin
        {
            get { return skin; }
            set { skin = value; }
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

        private bool twoFactorEnabled = false;

        public bool TwoFactorEnabled
        {
            get { return twoFactorEnabled; }
            set { twoFactorEnabled = value; }
        }

        private string webSiteUrl = string.Empty;

        public string WebSiteUrl
        {
            get { return webSiteUrl; }
            set { webSiteUrl = value; }
        }

        private string windowsLiveID = string.Empty;

        public string WindowsLiveId
        {
            get { return windowsLiveID; }
            set { windowsLiveID = value; }
        }

        


        //private string occupation = string.Empty;
        //private string interests = string.Empty;
        //private string msn = string.Empty;
        //private string yahoo = string.Empty;
        //private string aim = string.Empty;
        //private string icq = string.Empty;

        //private double timeOffsetHours = 0;
        //private bool nonLazyLoadedProfilePropertiesLoaded = false;
        //private bool rolesLoaded = false;

        

    }
}
