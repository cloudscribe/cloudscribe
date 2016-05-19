// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:				    2016-01-16
// Last Modified:		    2016-05-19
// 

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using Xunit;

namespace cloudscribe.Core.Web.Tests
{
    public class DataProtectionTests
    {

        private IServiceProvider serviceProvider = null;
        private IDataProtectionProvider dataProtectionProvider = null;
        private IDataProtector rawProtector = null;
        private IPersistedDataProtector persistentProtector = null;
        bool didSetup = false;

        private void Setup()
        {
            var services = new ServiceCollection();

            //http://docs.asp.net/en/latest/security/data-protection/configuration/overview.html
            //If you change the key persistence location, the system will no longer automatically encrypt keys 
            // at rest since it doesn’t know whether DPAPI is an appropriate encryption mechanism.
            //services.ConfigureDataProtection(configure =>
            //{
               
                //string pathToCryptoKeys = @"C:\_joe\__projects\__cloudscribe\_code\cloudscribe\src\example.WebApp\dp_keys\";

                // these keys are not encrypted at rest
                // since we have specified a non default location
                // that also makes the key portable so they will still work if we migrate to 
                // a new machine (will they work on different OS? I think so)
                // this is a similar server migration issue as the old machinekey
                // where we specified a machinekey in web.config so it would not change if we migrate to a new server
                //configure.PersistKeysToFileSystem(
                //    new DirectoryInfo(pathToCryptoKeys)
                //    );

                //configure.ProtectKeysWithCertificate("thumbprint");
                //configure.SetDefaultKeyLifetime(TimeSpan.FromDays(14));
                ///configure.
            //});

            //IDataProtectionProvider dataProtectionProvider



            services.AddDataProtection();

            serviceProvider = services.BuildServiceProvider();

            dataProtectionProvider = serviceProvider.GetService<IDataProtectionProvider>();
            rawProtector = dataProtectionProvider.CreateProtector("sts.Licensing.Web.KeyPairManager");
            persistentProtector = rawProtector as IPersistedDataProtector;

            didSetup = true;
        }

        [Fact]
        public void Can_Get_DataProtector_From_Services()
        {
            if (!didSetup) { Setup(); }

            Assert.NotNull(serviceProvider);

            Assert.NotNull(dataProtectionProvider);

        }

        [Fact]
        public void Can_Cast_DataProtector_As_IPersistedDataProtector()
        {
            if (!didSetup) { Setup(); }

            Assert.NotNull(rawProtector);

            Assert.NotNull(persistentProtector);

        }

        [Fact]
        public void Can_Round_Trip_Protection()
        {
            if (!didSetup) { Setup(); }

            Assert.NotNull(persistentProtector);

            string clearText = "Mozart was a fine violin player and composer";
            string protectedText = persistentProtector.Protect(clearText);

            string unprotectedText = persistentProtector.Unprotect(protectedText);

            Assert.True(clearText == unprotectedText);

        }

        [Fact]
        public void Can_Round_Trip_WithDangerous_Unprotect()
        {
            if (!didSetup) { Setup(); }

            Assert.NotNull(persistentProtector);

            string clearText = "Mozart was a fine violin player and composer";
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            string protectedText = persistentProtector.Protect(clearText);
            byte[] protectedBytes = persistentProtector.Protect(clearBytes);
            // this one fails so we can't expect to pass in string for Protect and then be
            // able to DangerousUnprotect
            //byte[] protectedBytes = Convert.FromBase64String(protectedText);

            bool ignoreRevocation = true;
            bool requiresMigration = false;
            bool wasRevoked = false;
            byte[] unprotectedBytes = persistentProtector.DangerousUnprotect(
                protectedBytes,
                ignoreRevocation,
                out requiresMigration,
                out wasRevoked);

            string unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);

            Assert.True(clearText == unprotectedText);

        }

        [Fact]
        public void Can_Round_Trip_Persistent()
        {
            if (!didSetup) { Setup(); }

            Assert.NotNull(persistentProtector);

            string clearText = "Mozart was a fine violin player and composer";

            string protectedText = persistentProtector.PersistentProtect(clearText);

            bool requiresMigration = false;
            bool wasRevoked = false;
            string unprotectedText = persistentProtector.PersistentUnprotect(
                protectedText,
                out requiresMigration,
                out wasRevoked);

            Assert.True(clearText == unprotectedText);

        }

    }
}
