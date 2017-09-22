using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    /// <summary>
    /// No operation analytics doesn't do anything, register this as scoped if you don't want any analytics
    /// </summary>
    public class NoopAccountAnalytics : IHandleAccountAnalytics
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
    }
}
