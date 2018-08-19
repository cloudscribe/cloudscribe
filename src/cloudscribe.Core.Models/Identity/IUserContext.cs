using System;

namespace cloudscribe.Core.Models
{
    public interface IUserContext
    {
        Guid Id { get; }
        Guid SiteId { get; }
        string UserName { get; }
        string DisplayName { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        bool EmailConfirmed { get; }
        DateTime? EmailConfirmSentUtc { get; }
        DateTime? AgreementAcceptedUtc { get; }
        DateTime CreatedUtc { get; }
        DateTime LastModifiedUtc { get; }
        DateTime? DateOfBirth { get; }
        bool DisplayInMemberList { get; }
        string WebSiteUrl { get;  }
        
        bool IsLockedOut { get; }

        DateTime? LastLoginUtc { get; }
        string TimeZoneId { get; }

        string PhoneNumber { get;  }
        bool PhoneNumberConfirmed { get; }
        bool AccountApproved { get;  }
        string AvatarUrl { get;  }
        string Gender { get;  }
        bool RolesChanged { get; }

        bool MustChangePwd { get; }
    }
}
