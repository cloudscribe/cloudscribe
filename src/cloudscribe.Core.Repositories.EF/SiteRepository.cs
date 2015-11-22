// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-22
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{

    public class SiteRepository
    {
        public SiteRepository(CoreDbContext dbContext)
        {

            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

        public async Task<bool> Save(ISiteSettings site)
        {
            if(site == null) { return false; }

            SiteSettings siteSettings = SiteSettings.FromISiteSettings(site); 
            dbContext.Sites.Add(siteSettings);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<ISiteSettings> Fetch(int siteId)
        {
            SiteSettings item
                = await dbContext.Sites.FirstOrDefaultAsync(x => x.SiteId == siteId);

            return item;
        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            SiteSettings item
                = dbContext.Sites.FirstOrDefault(x => x.SiteId == siteId);

            return item;
        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            SiteSettings item
                = await dbContext.Sites.FirstOrDefaultAsync(x => x.SiteGuid == siteGuid);

            return item;
        }

        public ISiteSettings FetchNonAsync(Guid siteGuid)
        {
            SiteSettings item
                = dbContext.Sites.FirstOrDefault(x => x.SiteGuid == siteGuid);

            return item;
        }

        public async Task<ISiteSettings> Fetch(string hostName)
        {
            SiteHost host = await dbContext.SiteHosts.FirstOrDefaultAsync(x => x.HostName == hostName);
            if(host == null)
            {
                var query = from s in dbContext.Sites
                            .Take(1)
                            orderby s.SiteId ascending
                            select s;

                return await query.FirstAsync<SiteSettings>();
            }
            
            return await dbContext.Sites.FirstOrDefaultAsync(x => x.SiteId == host.SiteId);


        }

        public ISiteSettings FetchNonAsync(string hostName)
        {
            SiteHost host = dbContext.SiteHosts.FirstOrDefault(x => x.HostName == hostName);
            if (host == null)
            {
                var query = from s in dbContext.Sites
                            .Take(1)
                            orderby s.SiteId ascending
                            select s;

                return query.First<SiteSettings>();
            }

            return dbContext.Sites.FirstOrDefault(x => x.SiteId == host.SiteId);

        }

        public async Task<bool> Delete(int siteId)
        {
            var result = false;
            var itemToRemove = await dbContext.Sites.FirstOrDefaultAsync(x => x.SiteId == siteId);
            if (itemToRemove != null)
            {
                dbContext.Sites.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }


        public async Task<int> GetCount()
        {
            return await dbContext.Sites.CountAsync<SiteSettings>();
        }




    }
}
