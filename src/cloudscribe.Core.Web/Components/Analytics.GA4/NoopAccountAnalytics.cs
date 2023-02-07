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

        public Task HandleLogout(string reason)
        {
            return Task.FromResult(0);
        }
    }
}
