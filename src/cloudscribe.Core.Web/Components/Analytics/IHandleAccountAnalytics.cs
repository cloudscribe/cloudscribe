using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    public interface IHandleAccountAnalytics
    {
        Task HandleLoginSubmit(string source);
        Task HandleLoginFail(string source, string reason);
        Task HandleRegisterSubmit(string source);
        Task HandleRegisterFail(string source, string reason);
        Task HandleLoginSuccess(UserLoginResult result);
        Task HandleLoginNotAllowed(UserLoginResult result);
        Task HandleRequiresTwoFactor(UserLoginResult result);
        Task HandleLockout(UserLoginResult result);

    }
}
