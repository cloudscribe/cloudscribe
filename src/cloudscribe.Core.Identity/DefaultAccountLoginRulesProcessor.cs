// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-25
// Last Modified:			2017-05-26
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class DefaultAccountLoginRulesProcessor : IProcessAccountLoginRules
    {
        public DefaultAccountLoginRulesProcessor(
            SiteUserManager<SiteUser> userManager,
            ISiteAcountCapabilitiesProvider capabilitiesProvider
            )
        {
            this.userManager = userManager;
            this.capabilitiesProvider = capabilitiesProvider;
        }

        private readonly SiteUserManager<SiteUser> userManager;
        private readonly ISiteAcountCapabilitiesProvider capabilitiesProvider;
        

        private async Task<bool> RequireConfirmedEmail()
        {
            if (!userManager.Site.RequireConfirmedEmail) return false;
            // if true check if it is possible, it is only possible to confirm email if email notification is setup
            return await capabilitiesProvider.SupportsEmailNotification(userManager.Site);
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
                    template.EmailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(template.User);
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (userManager.Site.RequireApprovalBeforeLogin)
            {
                if (!template.User.AccountApproved)
                {
                    var reason = $"login not allowed for {template.User.Email} because account not approved yet";
                    template.RejectReasons.Add(reason);
                    template.NeedsAccountApproval = true;
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (userManager.Site.RequireConfirmedPhone && userManager.Site.SmsIsConfigured())
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

            if (!string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement))
            {
                // need to not block login otherwise how can we make the user agree agree to terms on account
                // enforced with middleware
                if (template.User.AgreementAcceptedUtc == null || template.User.AgreementAcceptedUtc < userManager.Site.TermsUpdatedUtc)
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

            if (template.User.IsDeleted)
            {
                var reason = $"login not allowed for {template.User.Email} because account is flagged as deleted";
                template.RejectReasons.Add(reason);
                template.User = null;


            }
        }
    }
}
