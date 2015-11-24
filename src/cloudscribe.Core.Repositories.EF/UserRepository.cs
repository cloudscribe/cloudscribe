// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-24
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public class UserRepository
    {
        public UserRepository(CoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;


        public async Task<bool> Save(ISiteUser user)
        {
            if (user == null) { return false; }
            if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            dbContext.Users.Add(siteUser);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> Delete(int userId)
        {
            var result = false;
            var itemToRemove = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);
            if (itemToRemove != null)
            {
                dbContext.Users.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }




    }
}
