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
    /// these are the events that can be handled by the GA4 analytics implementation
    /// </summary>
    public partial interface IHandleAccountAnalytics
    {
        Task HandleGA4LoginSubmit(string source);
        Task HandleGA4LoginFail(string source, string reason);
        Task HandleGA4RegisterSubmit(string source);
        Task HandleGA4RegisterFail(string source, string reason);
        Task HandleGA4LoginSuccess(UserLoginResult result);
        Task HandleGA4LoginNotAllowed(UserLoginResult result);
        Task HandleGA4RequiresTwoFactor(UserLoginResult result);
        Task HandleGA4Lockout(UserLoginResult result);
        Task HandleGA4Logout(string reason);
        Task HandleGA4Search(string searchQuery, int numResults);
        /// <summary>
        /// This is a generic event handler for any other event you want to track
        /// </summary>
        Task HandleGA4Event(string eventName, List<KeyValuePair<string,string>> parameters);

    }
}
