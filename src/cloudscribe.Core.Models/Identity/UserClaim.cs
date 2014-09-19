// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-19
// 




namespace cloudscribe.Core.Models
{
    public class UserClaim : IUserClaim
    {

        public UserClaim()
        { }

      
        private int id = -1;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string userId = string.Empty;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string ClaimType
        {
            get { return claimType; }
            set { claimType = value; }
        }

        private string claimType = string.Empty;

        private string claimValue = string.Empty;

        public string ClaimValue
        {
            get { return claimValue; }
            set { claimValue = value; }
        }

    }

}
