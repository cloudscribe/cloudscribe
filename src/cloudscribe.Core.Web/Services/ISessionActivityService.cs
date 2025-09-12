using System;

namespace cloudscribe.Core.Web.Services
{
    public interface ISessionActivityService
    {
        void UpdateActivity(string userId, string siteId);
        DateTime? GetSessionExpiry(string userId, string siteId);
        void RemoveSession(string userId, string siteId);
    }
}