// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-25
// Last Modified:			2017-05-26
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class DefaultAccountLoginRulesProcessor : IProcessAccountLoginRules
    {
        public DefaultAccountLoginRulesProcessor(
            SiteUserManager<SiteUser> userManager,
            ISiteAccountCapabilitiesProvider capabilitiesProvider,
            IStringLocalizer<DefaultAccountLoginRulesProcessor> stringLocalizer
            )
        {
            _userManager = userManager;
            _capabilitiesProvider = capabilitiesProvider;
            _sr = stringLocalizer;
        }

        private readonly SiteUserManager<SiteUser> _userManager;
        private readonly ISiteAccountCapabilitiesProvider _capabilitiesProvider;
        private IStringLocalizer<DefaultAccountLoginRulesProcessor> _sr;

        private async Task<bool> RequireConfirmedEmail()
        {
            if (!_userManager.Site.RequireConfirmedEmail) return false;
            // it is only possible to confirm email if email notification is setup
            return await _capabilitiesProvider.SupportsEmailNotification(_userManager.Site);
        }

        public async Task ProcessAccountLoginRules(LoginResultTemplate template)
        {
            if (template.User == null) return;
            var requireConfirmedEmail = await RequireConfirmedEmail();

            if (requireConfirmedEmail)
            {
                if (!template.User.EmailConfirmed)
                {
                    var reason = $"login not allowed for {template.User.Email} because email is not confirmed";
                    template.RejectReasons.Add(reason);
                    template.NeedsEmailConfirmation = true;
                    template.EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(template.User);
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (_userManager.Site.RequireApprovalBeforeLogin)
            {
                if (!template.User.AccountApproved)
                {
                    var reason = $"login not allowed for {template.User.Email} because account not approved yet";
                    template.RejectReasons.Add(reason);
                    template.NeedsAccountApproval = true;
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (_userManager.Site.RequireConfirmedPhone && _userManager.Site.SmsIsConfigured())
            {
                if (string.IsNullOrEmpty(template.User.PhoneNumber))
                {
                    // we can't add a reason here that would block login
                    // we need to enforce user to add phone number via middleware redirect
                    // not by blocking login
                    // because without logging in user cannot update the phone
                    template.NeedsPhoneConfirmation = true;
                }
                else
                {
                    if (!template.User.PhoneNumberConfirmed)
                    {
                        var reason = $"login not allowed for {template.User.Email} because phone not added or verified yet";
                        template.RejectReasons.Add(reason);
                        template.NeedsPhoneConfirmation = true;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(_userManager.Site.RegistrationAgreement))
            {
                // need to not block login otherwise how can we make the user agree agree to terms on account
                // enforced with middleware
                if (template.User.AgreementAcceptedUtc == null || template.User.AgreementAcceptedUtc < _userManager.Site.TermsUpdatedUtc)
                {
                    template.MustAcceptTerms = true;
                }
            }

            if (template.User.IsLockedOut)
            {
                var reason = $"login not allowed for {template.User.Email} because account is locked out";
                template.RejectReasons.Add(reason);
                template.SignInResult = SignInResult.LockedOut;
            }

            if (template.User.CanAutoLockout && template.User.LockoutEndDateUtc != null && template.User.LockoutEndDateUtc > DateTime.UtcNow)
            {
                var reason = $"login is temporarily blocked for {template.User.Email} due to multiple failed login attempts";
                template.RejectReasons.Add(reason);
                template.SignInResult = SignInResult.LockedOut;
            }

            if (template.User.LastPasswordChangeUtc != null)
            {
                int passwordExpiryWarningDays = _capabilitiesProvider.GetPasswordExpiryWarningDays(_userManager.Site);
                int daysSinceLastPasswordChange = (DateTime.UtcNow - (DateTime)template.User.LastPasswordChangeUtc).Days;
                int passwordExpiryDays = _capabilitiesProvider.GetPasswordExpiryDays(_userManager.Site);

                if (passwordExpiryDays == 0)
                {
                    return;
                }

                if (daysSinceLastPasswordChange > passwordExpiryDays)
                {
                    var reason = _sr["please check your details and try again, or use the forgot password link"];
                    template.RejectReasons.Add(reason);
                    template.SignInResult = SignInResult.Failed;
                }

                if (daysSinceLastPasswordChange >= passwordExpiryWarningDays && daysSinceLastPasswordChange < passwordExpiryDays)
                {
                    template.PasswordExpiryReminder = _sr["Your password will expire in {0} day(s). It is recommended you change it now.", passwordExpiryDays - daysSinceLastPasswordChange];
                }

                if (daysSinceLastPasswordChange >= passwordExpiryWarningDays && daysSinceLastPasswordChange == passwordExpiryDays)
                {
                    template.PasswordExpiryReminder =  _sr["Your password will expire today! Please change it now."];
                }
            }
        }
    }
}
