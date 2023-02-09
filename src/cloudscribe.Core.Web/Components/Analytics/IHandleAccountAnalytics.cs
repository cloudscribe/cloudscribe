using cloudscribe.Core.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    /// <summary>
    /// These are the events that can be handled by analytics
    /// and will switch their behaviour depending on the ProfileId (UA- for old analytics, G- for GA4)
    /// </summary>
    public partial interface IHandleAccountAnalytics
    {
        Task HandleLoginSubmit(string source);
        Task HandleLoginFail(string source, string reason);
        Task HandleRegisterSubmit(string source);
        Task HandleRegisterFail(string source, string reason);
        Task HandleLoginSuccess(UserLoginResult result);
        Task HandleLoginNotAllowed(UserLoginResult result);
        Task HandleRequiresTwoFactor(UserLoginResult result);
        Task HandleLockout(UserLoginResult result);
        // added new handler for logouts - not implemented in UA, but is in GA4
        Task HandleLogout(string reason);
        // added new handler for search - not implemented in UA, but is in GA4
        Task HandleSearch(string searchQuery, int numResults);
        /// <summary>
        /// This is a generic event handler for any custom event you want to track - not implemented in UA, but is in GA4
        /// </summary>
        public Task HandleEvent(string eventName, List<KeyValuePair<string,string>> parameters);
    }
}
