// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:				    2019-09-01
// Last Modified:		    2019-09-01
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteUserConfirmation<TUser> : IUserConfirmation<TUser> where TUser : SiteUser
    {
        public Task<bool> IsConfirmedAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }
    }
}
