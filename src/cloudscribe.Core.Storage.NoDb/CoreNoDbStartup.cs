

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Hosting // so it will show up in startup without a using
{
    public static class CoreNoDbStartup
    {
        public static async Task InitializeDataAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var siteQueries = serviceScope.ServiceProvider.GetService<ISiteQueries>();
                var siteCommands = serviceScope.ServiceProvider.GetService<ISiteCommands>();
                var userQueries = serviceScope.ServiceProvider.GetService<IUserQueries>();
                var userCommands = serviceScope.ServiceProvider.GetService<IUserCommands>();
                var geoQueries = serviceScope.ServiceProvider.GetService<IGeoQueries>();
                var geoCommands = serviceScope.ServiceProvider.GetService<IGeoCommands>();

                await EnsureData(
                    siteQueries,
                    siteCommands,
                    userQueries,
                    userCommands,
                    geoQueries,
                    geoCommands
                    );

            }
        }


        private static async Task EnsureData(
            ISiteQueries siteQueries,
            ISiteCommands siteCommands,
            IUserQueries userQueries,
            IUserCommands userCommands,
            IGeoQueries geoQueries,
            IGeoCommands geoCommands
            )
        {
            
            int count = await geoQueries.GetCountryCount();
            if (count == 0)
            {
                foreach (GeoCountry c in InitialData.BuildCountryList())
                {
                    await geoCommands.Add(c);
                }

                foreach (GeoZone c in InitialData.BuildStateList())
                {
                    await geoCommands.Add(c);
                }
            }



            count = await geoQueries.GetLanguageCount();
            if (count == 0)
            {
                foreach (Language c in InitialData.BuildLanguageList())
                {
                    await geoCommands.Add(c);
                }

               
            }

            var all = await geoQueries.GetAllCurrencies();
            count = all.Count;
            if (count == 0)
            {
                foreach (Currency c in InitialData.BuildCurrencyList())
                {
                    await geoCommands.Add(c);
                }
                
            }


            count = await siteQueries.GetCount();
            SiteSettings newSite = null;
            if (count == 0)
            {
                // create first site
                newSite = new SiteSettings();
                //newSite.SiteId = 0;
                newSite.Id = Guid.NewGuid();
                newSite.AliasId = "tenant-" + (count + 1).ToInvariantString();
                newSite.SiteName = "Sample Site";
                newSite.IsServerAdminSite = true;

                newSite.Theme = "default";

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

                await siteCommands.Create(newSite);
                

            }

            // ensure roles
            if (newSite == null) return;

            count = await userQueries.CountOfRoles(newSite.Id, string.Empty);
            if (count == 0)
            {
                SiteRole adminRole = new SiteRole();
                adminRole.RoleName = "Admins";
                adminRole.DisplayName = "Administrators";
                adminRole.SiteId = newSite.Id;
                await userCommands.CreateRole(adminRole);
                

                SiteRole roleAdminRole = new SiteRole();
                roleAdminRole.RoleName = "Role Admins";
                roleAdminRole.DisplayName = "Role Administrators";
                roleAdminRole.SiteId = newSite.Id;
                await userCommands.CreateRole(roleAdminRole);
                

                SiteRole contentAdminRole = new SiteRole();
                contentAdminRole.RoleName = "Content Administrators";
                contentAdminRole.DisplayName = "Content Administrators";
                contentAdminRole.SiteId = newSite.Id;
                await userCommands.CreateRole(contentAdminRole);

                SiteRole authenticatedUserRole = new SiteRole();
                authenticatedUserRole.RoleName = "Authenticated Users";
                authenticatedUserRole.DisplayName = "Authenticated Users";
                authenticatedUserRole.SiteId = newSite.Id;
                await userCommands.CreateRole(authenticatedUserRole);
                
            }

            // ensure admin user
            count = await userQueries.CountUsers(newSite.Id, string.Empty);

            if (count == 0)
            {
                var role = await userQueries.FetchRole(newSite.Id, "Admins");
                
                if (role != null)
                {
                    SiteUser adminUser = new SiteUser();
                    adminUser.SiteId = newSite.Id;
                    adminUser.Email = "admin@admin.com";
                    adminUser.NormalizedEmail = adminUser.Email;
                    adminUser.DisplayName = "Admin";
                    adminUser.UserName = "admin";

                    adminUser.EmailConfirmed = true;
                    adminUser.AccountApproved = true;

                    // clear text password will be hashed upon login
                    // this format allows migrating from mojoportal
                    adminUser.PasswordHash = "admin||0"; //pwd/salt/format 

                    await userCommands.Create(adminUser);
                    
                    await userCommands.AddUserToRole(role.Id, adminUser.Id);

                    role = await userQueries.FetchRole(newSite.Id, "Authenticated Users");
                    
                    if (role != null)
                    {
                        await userCommands.AddUserToRole(role.Id, adminUser.Id);
                    }


                    
                }

                

            }

        }

    }
}
