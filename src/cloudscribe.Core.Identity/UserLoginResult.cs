// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-09-21
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class UserLoginResult
    {
        public UserLoginResult(
            SignInResult signInResult,
            List<string> rejectReasons,
            IUserContext user,
            bool isNewUserRegistration,
            bool mustAcceptTerms,
            bool needsAccountApproval,
            bool needsEmailConfirmation,
            string emailConfirmationToken,
            bool needsPhoneConfirmation,
            string passwordExpiryReminder,
            ExternalLoginInfo externalLoginInfo = null,
            bool isNewExternalAuthMapping = false
            )
        {
            SignInResult = signInResult;
            
            RejectReasons = rejectReasons;
            if (RejectReasons == null)
            {
                RejectReasons = new List<string>();
            }
            User = user;
            IsNewUserRegistration = isNewUserRegistration;
            MustAcceptTerms = mustAcceptTerms;
            NeedsAccountApproval = needsAccountApproval;
            NeedsEmailConfirmation = needsEmailConfirmation;
            EmailConfirmationToken = emailConfirmationToken;
            NeedsPhoneConfirmation = needsPhoneConfirmation;
            ExternalLoginInfo = externalLoginInfo;
            IsNewExternalAuthMapping = isNewExternalAuthMapping;
            PasswordExpiryReminder = passwordExpiryReminder;
    }

        public bool MustAcceptTerms { get; }

        public bool NeedsAccountApproval { get; }

        public bool NeedsEmailConfirmation { get; }

        public string EmailConfirmationToken { get; }

        public bool NeedsPhoneConfirmation { get; }

        public ExternalLoginInfo ExternalLoginInfo { get; }
        
        public SignInResult SignInResult { get; }

        public List<string> RejectReasons { get; }

        public IUserContext User { get; }

        public bool IsNewUserRegistration { get; }

        /// <summary>
        /// Are we adding a new external UserLogin mapping to an existing cloudscribe account
        /// - thereby requiring a confirmation
        /// </summary>
        public bool IsNewExternalAuthMapping { get; }
        public string PasswordExpiryReminder { get; set; }
    }

}
