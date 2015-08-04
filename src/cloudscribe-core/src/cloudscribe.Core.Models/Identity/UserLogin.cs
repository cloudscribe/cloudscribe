// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-08-04
// 

namespace cloudscribe.Core.Models
{
    public class UserLogin : IUserLogin
    {
        public UserLogin()
        { }

        public string LoginProvider { get; set; } = string.Empty;    
        public string ProviderKey { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;


    }

}
