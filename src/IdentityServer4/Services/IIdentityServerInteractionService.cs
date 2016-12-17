// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    public interface IIdentityServerInteractionService
    {
        Task<AuthorizationRequest> GetAuthorizationContextAsync(string returnUrl);
        Task<ErrorMessage> GetErrorContextAsync(string errorId);

        Task<LogoutRequest> GetLogoutContextAsync(string logoutId);
        Task<string> CreateLogoutContextAsync();

        bool IsValidReturnUrl(string returnUrl);
        Task GrantConsentAsync(AuthorizationRequest request, ConsentResponse consent, string subject = null);

        Task<IEnumerable<Consent>> GetAllUserConsentsAsync();
        Task RevokeUserConsentAsync(string clientId);
        Task RevokeTokensForCurrentSessionAsync();
    }
}
