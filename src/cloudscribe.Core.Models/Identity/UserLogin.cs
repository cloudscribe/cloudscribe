// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-12-08
// 

namespace cloudscribe.Core.Models
{
    public class UserLogin : IUserLogin
    {
        public UserLogin()
        { }

        public int SiteId { get; set; } = -1;

        private string loginProvider = string.Empty;
        public string LoginProvider
        {
            get { return loginProvider ?? string.Empty; }
            set { loginProvider = value; }
        }

        private string providerKey = string.Empty;
        public string ProviderKey
        {
            get { return providerKey ?? string.Empty; }
            set { providerKey = value; }
        }

        private string providerDisplayName = string.Empty;
        public string ProviderDisplayName
        {
            get { return providerDisplayName ?? string.Empty; }
            set { providerDisplayName = value; }
        }

        private string userId = string.Empty;
        public string UserId
        {
            get { return userId ?? string.Empty; }
            set { userId = value; }
        }
        
       
        public static UserLogin FromIUserLogin(IUserLogin i)
        {
            UserLogin l = new UserLogin();

            l.LoginProvider = i.LoginProvider;
            l.ProviderDisplayName = i.ProviderDisplayName;
            l.ProviderKey = i.ProviderKey;
            l.SiteId = i.SiteId;
            l.UserId = i.UserId;

            return l;
        }

    }

}
