// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2017-08-03
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;


namespace Microsoft.AspNetCore.Hosting // so it will show up in startup without a using
{
    public static class CoreEFStartup
    {

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            var siteConfigAccessor = serviceProvider.GetService<IOptions<SiteConfigOptions>>();
            var db = serviceProvider.GetService<ICoreDbContext>();
            await db.Database.MigrateAsync();
            await EnsureData(db, siteConfigAccessor.Value);

        }

        private static async Task EnsureData(
            ICoreDbContext db,
            SiteConfigOptions config
            )
        {
            int rowsAffected = 0;
            
            int count = await db.Countries.CountAsync<GeoCountry>();
            if(count == 0)
            {
                foreach(GeoCountry c in InitialData.BuildCountryList())
                {
                    db.Countries.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }
            
            count = await db.States.CountAsync<GeoZone>();
            if (count == 0)
            {
                foreach (GeoZone c in InitialData.BuildStateList())
                {
                    db.States.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }
 
            count = await db.Sites.CountAsync<SiteSettings>();
            SiteSettings newSite = null;
            if (count == 0)
            {
                // create first site
                newSite = InitialData.BuildInitialSite();
                newSite.Theme = config.FirstSiteTheme;
                
                db.Sites.Add(newSite);
                
                rowsAffected = await db.SaveChangesAsync();
                   
            }

            // ensure roles
            count = await db.Roles.CountAsync<SiteRole>();
            if (count == 0)
            {
                var site = newSite;
                if(site == null)
                {
                    site = await db.Sites.SingleOrDefaultAsync<SiteSettings>(
                        s => s.Id != Guid.Empty && s.IsServerAdminSite == true);
                }
                

                if(site != null)
                {
                    var adminRole = InitialData.BuildAdminRole();
                    adminRole.SiteId = site.Id;
                    db.Roles.Add(adminRole);

                    var roleAdminRole = InitialData.BuildRoleAdminRole();
                    roleAdminRole.SiteId = site.Id;
                    db.Roles.Add(roleAdminRole);

                    var contentAdminRole = InitialData.BuildContentAdminsRole();
                    contentAdminRole.SiteId = site.Id;
                    db.Roles.Add(contentAdminRole);

                    var authenticatedUserRole = InitialData.BuildAuthenticatedRole();
                    authenticatedUserRole.SiteId = site.Id;
                    db.Roles.Add(authenticatedUserRole);
                    
                    rowsAffected = await db.SaveChangesAsync();
                    
                }

            }

            // ensure admin user
            count = await db.Users.CountAsync<SiteUser>();
            
            if (count == 0)
            {
                SiteSettings site = await db.Sites.FirstOrDefaultAsync<SiteSettings>(
                    s => s.Id != Guid.Empty && s.IsServerAdminSite == true);
                    
                if (site != null)
                {
                    var role = await db.Roles.FirstOrDefaultAsync(
                            x => x.SiteId == site.Id && x.NormalizedRoleName == "ADMINISTRATORS");

                    if(role != null)
                    {
                        var adminUser = InitialData.BuildInitialAdmin();
                        adminUser.SiteId = site.Id;
                        adminUser.Id = Guid.NewGuid();
                        adminUser.CanAutoLockout = false;
                        db.Users.Add(adminUser);
                        
                        rowsAffected = await db.SaveChangesAsync();
                        
                        if(rowsAffected > 0 && adminUser.Id != Guid.Empty)
                        {
                            var ur = new UserRole();
                            ur.RoleId = role.Id;
                            ur.UserId = adminUser.Id;
                            db.UserRoles.Add(ur);
                            await db.SaveChangesAsync();

                            role = await db.Roles.SingleOrDefaultAsync(
                                 x => x.SiteId == site.Id && x.NormalizedRoleName == "Authenticated Users".ToUpperInvariant());

                            if(role != null)
                            {
                                ur = new UserRole();
                                ur.RoleId = role.Id;
                                ur.UserId = adminUser.Id;
                                db.UserRoles.Add(ur);
                                await db.SaveChangesAsync();
                            }
                            

                        }
                    }

                }

            }
            
        }

        
    }
}
