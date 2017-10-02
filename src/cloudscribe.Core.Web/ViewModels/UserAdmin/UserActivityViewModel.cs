using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class UserActivityViewModel
    {
        public UserActivityViewModel()
        {
            Locations = new PagedResult<IUserLocation>();
        }

        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedUtc { get; set; }

        public DateTime? LastLoginUtc { get; set; }
        public DateTime? LastPassswordChangenUtc { get; set; }

        public PagedResult<IUserLocation> Locations { get; set; }

        public string TimeZoneId { get; set; } = "America/New_York";
    }
}
