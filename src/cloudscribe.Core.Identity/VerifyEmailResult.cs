// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-23
// Last Modified:			2017-05-23
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    public class VerifyEmailResult
    {
        public VerifyEmailResult(
            IUserContext user,
            IdentityResult identityResult
            )
        {
            User = user;
            IdentityResult = identityResult;
        }

        public IUserContext User { get; }
        public IdentityResult IdentityResult { get; }
    }
}
