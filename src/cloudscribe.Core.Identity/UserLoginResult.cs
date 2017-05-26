// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-05-24
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
            List<string> rejectReasons = null,
            IUserContext user = null,
            bool mustAcceptTerms = false,
            bool needsAccountApproval = false,
            bool needsEmailConfirmation = false,
            string emailConfirmationToken = "",
            bool needsPhoneConfirmation = false,
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
    }

}
