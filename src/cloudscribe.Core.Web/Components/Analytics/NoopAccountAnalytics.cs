using cloudscribe.Core.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    /// <summary>
    /// No operation analytics doesn't do anything, register this as scoped if you don't want any analytics
    /// </summary>
    public partial class NoopAccountAnalytics : IHandleAccountAnalytics
    {
        public Task HandleLoginSubmit(string source)
        {
            return Task.FromResult(0);
        }

        public Task HandleLoginFail(string source, string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleRegisterSubmit(string source)
        {
            return Task.FromResult(0);
        }

        public Task HandleRegisterFail(string source, string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleLoginSuccess(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleLoginNotAllowed(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleRequiresTwoFactor(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleLockout(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleLogout(string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleSearch(string searchQuery, int numResults)
        {
            return Task.FromResult(0);
        }

        public Task HandleEvent(string eventName, List<KeyValuePair<string,string>> parameters)
        {
            return Task.FromResult(0);
        }
    }
}
