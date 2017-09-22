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
            ExternalLoginInfo externalLoginInfo = null
            )
        {
            SignInResult = signInResult;
            
            RejectReasons = rejectReasons;
            if (RejectReasons != null)
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
            
        }

        public bool MustAcceptTerms { get; }

        public bool NeedsAccountApproval { get; }

        public bool NeedsEmailConfirmation { get; }

        public string EmailConfirmationToken { get; }

        public bool NeedsPhoneConfirmation { get; }

        public ExternalLoginInfo ExternalLoginInfo { get; }
        
        public SignInResult SignInResult { get; }

        /// <summary>
        /// these reasons  are not meant for display in the ui
        /// it is not a good idea to give reasons, in ui just show invalid login message without showing reason
        /// </summary>
        public List<string> RejectReasons { get; }

        public IUserContext User { get; }

        public bool IsNewUserRegistration { get; }
    }

}
