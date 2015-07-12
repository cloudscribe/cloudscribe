// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-19
// 

namespace cloudscribe.Core.Models
{
    public class UserLogin : IUserLogin
    {
        public UserLogin()
        { }

        private string loginProvider = string.Empty;

        public string LoginProvider
        {
            get { return loginProvider; }
            set { loginProvider = value; }
        }

        private string providerKey = string.Empty;

        public string ProviderKey
        {
            get { return providerKey; }
            set { providerKey = value; }
        }

        private string userId = string.Empty;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }



    }

}
