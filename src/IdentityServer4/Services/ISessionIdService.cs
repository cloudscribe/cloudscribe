// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace IdentityServer4.Services
{
    public interface ISessionIdService
    {
        void CreateSessionId(SignInContext context);
        Task<string> GetCurrentSessionIdAsync();

        Task EnsureSessionCookieAsync();
        string GetCookieName();
        string GetCookieValue();
        void RemoveCookie();
    }
}
