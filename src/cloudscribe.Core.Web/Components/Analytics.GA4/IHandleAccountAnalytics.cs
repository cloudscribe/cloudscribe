// Copyright (c) Idox Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

using cloudscribe.Core.Identity;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics.GA4
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
        Task HandleLogout(string reason);

    }
}
