// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models
{
    public class UserLogin : IUserLogin
    {
        public UserLogin()
        { }

        public Guid SiteId { get; set; } 
        
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ProviderDisplayName { get; set; }

        public Guid UserId { get; set; } = Guid.Empty;
        
        public static UserLogin FromIUserLogin(IUserLogin i)
        {
            UserLogin l = new UserLogin
            {
                LoginProvider = i.LoginProvider,
                ProviderDisplayName = i.ProviderDisplayName,
                ProviderKey = i.ProviderKey,
                SiteId = i.SiteId,
                UserId = i.UserId
            };

            return l;
        }

    }

}
