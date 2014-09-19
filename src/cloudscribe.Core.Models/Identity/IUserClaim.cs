using System;
namespace cloudscribe.Core.Models
{
    public interface IUserClaim
    {
        string ClaimType { get; set; }
        string ClaimValue { get; set; }
        int Id { get; set; }
        string UserId { get; set; }
    }
}
