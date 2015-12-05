// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2015-12-05
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;

namespace cloudscribe.Core.Repositories.EF
{
    public static class InitialData
    {

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<CoreDbContext>();
                
                if (await db.Database.EnsureCreatedAsync())
                {
                    await EnsureData(db);
 
                }
            }
        }

        private static async Task EnsureData(
            CoreDbContext db
            )
        {
           
                int count = await db.Countries.CountAsync<GeoCountry>();
                if(count == 0)
                {
                    foreach(GeoCountry c in cloudscribe.Core.Models.InitialData.BuildCountryList())
                    {
                        db.Countries.Add(c);
                    }
                }

                count = await db.States.CountAsync<GeoZone>();
                if (count == 0)
                {
                    foreach (GeoZone c in cloudscribe.Core.Models.InitialData.BuildStateList())
                    {
                        db.States.Add(c);
                    }
                }

                count = await db.Languages.CountAsync<Language>();
                if (count == 0)
                {
                    foreach (Language c in cloudscribe.Core.Models.InitialData.BuildLanguageList())
                    {
                        db.Languages.Add(c);
                    }
                }

                count = await db.Currencies.CountAsync<Currency>();
                if (count == 0)
                {
                    foreach (Currency c in cloudscribe.Core.Models.InitialData.BuildCurrencyList())
                    {
                        db.Currencies.Add(c);
                    }
                }

                // go ahead and save
                int rowsAffected = await db.SaveChangesAsync();

                count = await db.Sites.CountAsync<SiteSettings>();
                if (count == 0)
                {
                    // create first site
                    SiteSettings newSite = new SiteSettings();
                    newSite.SiteGuid = Guid.NewGuid();
                    newSite.SiteName = "Sample Site";
                    newSite.IsServerAdminSite = true;

                    newSite.Layout = "Default_Layout.cshtml";

                    newSite.AllowNewRegistration = true;
                    newSite.AllowUserFullNameChange = false;
                    newSite.AutoCreateLdapUserOnFirstLogin = true;
                    newSite.ReallyDeleteUsers = true;
                    newSite.LdapPort = 389;
                    newSite.LdapRootDN = String.Empty;
                    newSite.LdapServer = String.Empty;
                    newSite.UseEmailForLogin = true;
                    newSite.UseLdapAuth = false;
                    newSite.UseSecureRegistration = false;
                    newSite.UseSslOnAllPages = false;


                    //0 = clear, 1= hashed, 2= encrypted
                    //newSite.PasswordFormat = 1;

                    newSite.RequiresQuestionAndAnswer = false;
                    newSite.MaxInvalidPasswordAttempts = 10;
                    newSite.PasswordAttemptWindowMinutes = 5;
                    newSite.MinReqNonAlphaChars = 0;
                    newSite.MinRequiredPasswordLength = 7;

                    db.Sites.Add(newSite);

                    rowsAffected = await db.SaveChangesAsync();

                    
                }

                // ensure roles
                count = await db.Roles.CountAsync<SiteRole>();
                if (count == 0)
                {
                    SiteSettings site = await db.Sites.SingleOrDefaultAsync<SiteSettings>(
                        s => s.SiteId > -1);

                    if(site != null)
                    {
                        SiteRole adminRole = new SiteRole();
                        adminRole.DisplayName = "Admins";
                        //adminRole.DisplayName = "Administrators";
                        adminRole.SiteId = site.SiteId;
                        adminRole.SiteGuid = site.SiteGuid;
                        db.Roles.Add(adminRole);
                        rowsAffected = await db.SaveChangesAsync();
                        
                        adminRole.DisplayName = "Administrators";
                        db.Roles.Update(adminRole);
                        rowsAffected = await db.SaveChangesAsync();

                        SiteRole roleAdminRole = new SiteRole();
                        roleAdminRole.DisplayName = "Role Admins";
                        roleAdminRole.SiteId = site.SiteId;
                        roleAdminRole.SiteGuid = site.SiteGuid;
                        db.Roles.Add(roleAdminRole);
                        rowsAffected = await db.SaveChangesAsync();

                        roleAdminRole.DisplayName = "Role Administrators";
                        db.Roles.Update(roleAdminRole);
                        rowsAffected = await db.SaveChangesAsync();

                        SiteRole contentAdminRole = new SiteRole();
                        contentAdminRole.DisplayName = "Content Administrators";
                        contentAdminRole.SiteId = site.SiteId;
                        contentAdminRole.SiteGuid = site.SiteGuid;
                        db.Roles.Add(contentAdminRole);

                        SiteRole authenticatedUserRole = new SiteRole();
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
                        s => s.SiteId > -1);
                    
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
                            adminUser.DisplayName = "Admin";
                            adminUser.UserName = "admin";
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
