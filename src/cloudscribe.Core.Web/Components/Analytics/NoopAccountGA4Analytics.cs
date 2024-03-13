// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

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
        public Task HandleGA4LoginSubmit(string source)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4LoginFail(string source, string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4RegisterSubmit(string source)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4RegisterFail(string source, string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4LoginSuccess(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4LoginNotAllowed(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4RequiresTwoFactor(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4Lockout(UserLoginResult result)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4Logout(string reason)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4Search(string searchQuery, int numResults)
        {
            return Task.FromResult(0);
        }

        public Task HandleGA4Event(string eventName, List<KeyValuePair<string,string>> parameters)
        {
            return Task.FromResult(0);
        }
    }
}
