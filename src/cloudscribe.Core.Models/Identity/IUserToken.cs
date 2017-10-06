using System;

namespace cloudscribe.Core.Models
{
    public interface IUserToken
    {
        Guid SiteId { get; set; }
        Guid UserId { get; set; }
        string LoginProvider { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
