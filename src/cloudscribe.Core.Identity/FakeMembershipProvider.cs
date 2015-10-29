#if DNX451

using System.Web.Security;

namespace cloudscribe.Core.Identity
{
    public class FakeMembershipProvider : MembershipProvider
    {
        /// <summary>
        /// this is the only method we are using here for migrating mojoportal accounts
        /// called from SitePasswordHasher.cs
        /// </summary>
        /// <param name="bIn"></param>
        /// <returns></returns>
        public byte[] EncryptPasswordBytes(byte[] bIn)
        {
            return base.EncryptPassword(bIn);
        }


#region required base class virtual implementation methods 

        private string applicationName = "FakeMembershipProvider";

        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        private string description = string.Empty;
        public override string Description
        {
            get { return description; }
        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 7; }
        }

        string name = "FakeMembershipProvider";
        public override string Name
        {
            get { return name; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 5; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return string.Empty; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return true; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }


        public override MembershipUser CreateUser(
            string userName,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.UserRejected;
            return null;

        }

        public override bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            return false;
        }

        public override bool UnlockUser(string userName)
        {
            return false;
        }

        public override bool ValidateUser(string userName, string password)
        {
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {

        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(
            string userName,
            string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            return false;
        }



        public override string GetPassword(string userName, string passwordAnswer)
        {
            return string.Empty;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return null;
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetUserNameByEmail(string email)
        {
            return string.Empty;
        }

        public override string ResetPassword(string userName, string passwordAnswer)
        {
            return string.Empty;
        }

        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();
            return users;

        }

        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();
            return users;
        }

        public override MembershipUserCollection GetAllUsers(
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();
            return users;
        }




#endregion



    }
}
#endif
