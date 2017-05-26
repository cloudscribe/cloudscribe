using System;

namespace cloudscribe.Core.Models
{
    public class UserContext : IUserContext
    {
        public UserContext(ISiteUser user)
        {
            if (user == null) throw new ArgumentNullException("you must pass in an implementation of ISiteUser");
            this.user = user;
        }

        private ISiteUser user;
        public bool AccountApproved
        {
            get
            {
                return user.AccountApproved;
            }
        }

        public string AvatarUrl
        {
            get
            {
                return user.AvatarUrl;
            }
        }

        public DateTime CreatedUtc
        {
            get
            {
                return user.CreatedUtc;
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                return user.DateOfBirth;
            }
        }

        public bool DisplayInMemberList
        {
            get
            {
                return user.DisplayInMemberList;
            }
        }

        public string DisplayName
        {
            get
            {
                return user.DisplayName;
            }
        }

        public string Email
        {
            get
            {
                return user.Email;
            }
        }

        public bool EmailConfirmed
        {
            get { return user.EmailConfirmed; }
        }

        public DateTime? EmailConfirmSentUtc
        {
            get { return user.EmailConfirmSentUtc; }
        }

        public DateTime? AgreementAcceptedUtc
        {
            get { return user.AgreementAcceptedUtc; }
        }

        public string FirstName
        {
            get
            {
                return user.FirstName;
            }
        }

        public string Gender
        {
            get
            {
                return user.Gender;
            }
        }

        public Guid Id
        {
            get
            {
                return user.Id;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return user.IsDeleted;
            }
        }

        public bool IsLockedOut
        {
            get
            {
                return user.IsLockedOut;
            }
        }

        public DateTime? LastLoginUtc
        {
            get
            {
                return user.LastLoginUtc;
            }
        }

        public DateTime LastModifiedUtc
        {
            get
            {
                return user.LastModifiedUtc;
            }
        }

        public string LastName
        {
            get
            {
                return user.LastName;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return user.PhoneNumber;
            }
        }

        public bool PhoneNumberConfirmed
        {
            get
            {
                return user.PhoneNumberConfirmed;
            }
        }

        public Guid SiteId
        {
            get
            {
                return user.SiteId;
            }
        }

        public string TimeZoneId
        {
            get
            {
                return user.TimeZoneId;
            }
        }

        public bool Trusted
        {
            get
            {
                return user.Trusted;
            }
        }

        public string UserName
        {
            get
            {
                return user.UserName;
            }
        }

        public string WebSiteUrl
        {
            get
            {
                return user.WebSiteUrl;
            }
        }

        public bool RolesChanged
        {
            get { return user.RolesChanged; }
        }
    }
}
