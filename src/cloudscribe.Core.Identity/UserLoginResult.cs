// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-05-22
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class UserLoginResult
    {
        public UserLoginResult(
           // bool succeeded,
            SignInResult signInResult,
            List<string> rejectReasons = null,
            IUserContext user = null,
            bool mustAcceptTerms = false,
            bool needsAccountApproval = false,
            bool needsEmailConfirmation = false,
            bool needsPhoneConfirmation = false
            )
        {
            SignInResult = signInResult;
            MustAcceptTerms = mustAcceptTerms;
            NeedsAccountApproval = needsAccountApproval;
            NeedsEmailConfirmation = needsEmailConfirmation;
            NeedsPhoneConfirmation = needsPhoneConfirmation;
            RejectReasons = rejectReasons;
            if (RejectReasons != null)
            {
                RejectReasons = new List<string>();
            }

        }

        public bool MustAcceptTerms { get; }

        public bool NeedsAccountApproval { get; }

       public bool NeedsEmailConfirmation { get; }

        public bool NeedsPhoneConfirmation { get; }


        public SignInResult SignInResult { get; }

        /// <summary>
        /// these reasons  are not meant for display in the ui
        /// it is not a good idea to give reasons, in ui just show invalid login message without showing reason
        /// </summary>
        public List<string> RejectReasons { get; }

        public IUserContext User { get; }
    }

}
