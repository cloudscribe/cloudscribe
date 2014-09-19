using System;
namespace cloudscribe.Core.Models
{
    public interface ISiteRole
    {
        string DisplayName { get; set; }
        Guid RoleGuid { get; set; }
        int RoleId { get; set; }
        string RoleName { get; set; }
        Guid SiteGuid { get; set; }
        int SiteID { get; set; }
        int MemberCount { get; set; }
    }
}
