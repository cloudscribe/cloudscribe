using System;
namespace cloudscribe.Core.Models
{
    public interface IUserLogin
    {
        string LoginProvider { get; set; }
        string ProviderKey { get; set; }
        string UserId { get; set; }
    }
}
