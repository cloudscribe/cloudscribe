// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2016-01-27
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public static class InitialData
    {

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<CoreDbContext>();

                // ran into an error when I made a change to the model and tried to apply the migration
                // found the reason was this line:
                //bool didCreatedDb = await db.Database.EnsureCreatedAsync();
                // according to https://github.com/aspnet/EntityFramework/issues/3160
                // EnsureCreatedAsync totally bypasses migrations and just creates the schema for you, you can't mix this with migrations. 
                // EnsureCreated is designed for testing or rapid prototyping where you are ok with dropping and re-creating 
                // the database each time. If you are using migrations and want to have them automatically applied on app start, 
                // then you can use context.Database.Migrate() instead.
                
                try
                {
                    await db.Database.MigrateAsync();
                }
                catch { }
                    
              
                

                await EnsureData(db);

            }

            
        }

        private static async Task EnsureData(
            CoreDbContext db
            )
        {
            int rowsAffected = 0;


            int count = await db.Countries.CountAsync<GeoCountry>();
            if(count == 0)
            {
                foreach(GeoCountry c in cloudscribe.Core.Models.InitialData.BuildCountryList())
                {
                    db.Countries.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }
            
            count = await db.States.CountAsync<GeoZone>();
            if (count == 0)
            {
                foreach (GeoZone c in cloudscribe.Core.Models.InitialData.BuildStateList())
                {
                    db.States.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }

            

            count = await db.Languages.CountAsync<Language>();
            if (count == 0)
            {
                foreach (Language c in cloudscribe.Core.Models.InitialData.BuildLanguageList())
                {
                    db.Languages.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }

            count = await db.Currencies.CountAsync<Currency>();
            if (count == 0)
            {
                foreach (Currency c in cloudscribe.Core.Models.InitialData.BuildCurrencyList())
                {
                    db.Currencies.Add(c);
                }

                rowsAffected = await db.SaveChangesAsync();
            }

                
            count = await db.Sites.CountAsync<SiteSettings>();
            if (count == 0)
            {
                // create first site
                SiteSettings newSite = new SiteSettings();
                newSite.SiteId = 0;
                newSite.SiteGuid = Guid.NewGuid();
                newSite.SiteName = "Sample Site";
                newSite.IsServerAdminSite = true;

                newSite.Layout = "Default_Layout.cshtml";

                newSite.AllowNewRegistration = true;
                //newSite.AllowUserFullNameChange = false;
                newSite.AutoCreateLdapUserOnFirstLogin = true;
                newSite.ReallyDeleteUsers = true;
                newSite.LdapPort = 389;
                newSite.LdapRootDN = string.Empty;
                newSite.LdapServer = string.Empty;
                newSite.UseEmailForLogin = true;
                newSite.UseLdapAuth = false;
                newSite.RequireConfirmedEmail = false;
                //newSite.UseSslOnAllPages = false;


                //0 = clear, 1= hashed, 2= encrypted
                //newSite.PasswordFormat = 1;

                newSite.RequiresQuestionAndAnswer = false;
                newSite.MaxInvalidPasswordAttempts = 10;
                //newSite.PasswordAttemptWindowMinutes = 5;
                //newSite.MinReqNonAlphaChars = 0;
                newSite.MinRequiredPasswordLength = 7;

                db.Sites.Add(newSite);
                
                rowsAffected = await db.SaveChangesAsync();
                   
            }

            // ensure roles
            count = await db.Roles.CountAsync<SiteRole>();
            if (count == 0)
            {
                SiteSettings site = await db.Sites.SingleOrDefaultAsync<SiteSettings>(
                    s => s.SiteId > 0 && s.IsServerAdminSite == true);

                if(site != null)
                {
                    SiteRole adminRole = new SiteRole();
                    adminRole.RoleId = 0;
                    adminRole.RoleGuid = Guid.NewGuid();
                    adminRole.RoleName = "Admins";
                    adminRole.DisplayName = "Administrators";
                    adminRole.SiteId = site.SiteId;
                    adminRole.SiteGuid = site.SiteGuid;
                    db.Roles.Add(adminRole);
                    //rowsAffected = await db.SaveChangesAsync();
                        
                    SiteRole roleAdminRole = new SiteRole();
                    roleAdminRole.RoleId = 0;
                    roleAdminRole.RoleGuid = Guid.NewGuid();
                    roleAdminRole.RoleName = "Role Admins";
                    roleAdminRole.DisplayName = "Role Administrators";
                    roleAdminRole.SiteId = site.SiteId;
                    roleAdminRole.SiteGuid = site.SiteGuid;
                    db.Roles.Add(roleAdminRole);
                    //rowsAffected = await db.SaveChangesAsync();
                    
                    SiteRole contentAdminRole = new SiteRole();
                    contentAdminRole.RoleId = 0;
                    contentAdminRole.RoleGuid = Guid.NewGuid();
                    contentAdminRole.RoleName = "Content Administrators";
                    contentAdminRole.DisplayName = "Content Administrators";
                    contentAdminRole.SiteId = site.SiteId;
                    contentAdminRole.SiteGuid = site.SiteGuid;
                    db.Roles.Add(contentAdminRole);

                    SiteRole authenticatedUserRole = new SiteRole();
                    authenticatedUserRole.RoleId = 0;
                    authenticatedUserRole.RoleGuid = Guid.NewGuid();
                    authenticatedUserRole.RoleName = "Authenticated Users";
                    authenticatedUserRole.DisplayName = "Authenticated Users";
                    authenticatedUserRole.SiteId = site.SiteId;
                    authenticatedUserRole.SiteGuid = site.SiteGuid;
                    db.Roles.Add(authenticatedUserRole);

                    
                    rowsAffected = await db.SaveChangesAsync();
                    
                   
                }

            }

            // ensure admin user
            count = await db.Users.CountAsync<SiteUser>();
            
            if (count == 0)
            {
                SiteSettings site = await db.Sites.SingleOrDefaultAsync<SiteSettings>(
                    s => s.SiteId > 0 && s.IsServerAdminSite == true);
                    
                if (site != null)
                {
                    SiteRole role
                        = await db.Roles.SingleOrDefaultAsync(
                            x => x.SiteId == site.SiteId && x.RoleName == "Admins");

                    if(role != null)
                    {
                        SiteUser adminUser = new SiteUser();
                        adminUser.SiteId = site.SiteId;
                        adminUser.SiteGuid = site.SiteGuid;
                        adminUser.Email = "admin@admin.com";
                        adminUser.NormalizedEmail = adminUser.Email;
                        adminUser.DisplayName = "Admin";
                        adminUser.UserName = "admin";
                        adminUser.UserId = 0;
                        adminUser.UserGuid = Guid.NewGuid();

                        adminUser.EmailConfirmed = true;
                        adminUser.AccountApproved = true;

                        // clear text password will be hashed upon login
                        // this format allows migrating from mojoportal
                        adminUser.PasswordHash = "admin||0"; //pwd/salt/format 

                        db.Users.Add(adminUser);
                        
                        rowsAffected = await db.SaveChangesAsync();
                        
                        if(rowsAffected > 0 && adminUser.UserId > -1)
                        {
                            UserRole ur = new UserRole();
                            ur.Id = 0;
                            ur.RoleGuid = role.RoleGuid;
                            ur.RoleId = role.RoleId;
                            ur.UserGuid = adminUser.UserGuid;
                            ur.UserId = adminUser.UserId;

                            db.UserRoles.Add(ur);
                            rowsAffected = await db.SaveChangesAsync();

                        }
                    }

                }

            }
            
        }

        
    }
}
