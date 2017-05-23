// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-23
// Last Modified:			2017-05-23
// 

using cloudscribe.Core.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class TwoFactorInfo
    {
        public TwoFactorInfo(
            IUserContext user,
            IList<string> userFactors,
            string twoFactorToken
            )
        {
            User = user;
            UserFactors = userFactors;
            TwoFactorToken = twoFactorToken;
        }

        public IUserContext User { get; }
        public IList<string> UserFactors { get; }
        public string TwoFactorToken { get; }


    }
}
