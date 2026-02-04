// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:				    2016-01-16
// Last Modified:		    2018-06-08
// 2018-06-08 for some reason even though these tests pass locally
// it throws an error on appveyor saying: Could not load file or assembly 'Microsoft.AspNetCore.DataProtection
// https://ci.appveyor.com/project/joeaudette/cloudscribe/build/1.0.716
// commenting this file out for now

//using cloudscribe.Core.DataProtection;
//using Microsoft.AspNetCore.DataProtection;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Text;
//using Xunit;

//namespace cloudscribe.Core.Web.Tests
//{
//    public class DataProtectionTests
//    {

//        private IServiceProvider serviceProvider = null;
//        private IDataProtectionProvider dataProtectionProvider = null;
//        private IDataProtector rawProtector = null;
//        private IPersistedDataProtector persistentProtector = null;
//        bool didSetup = false;

//        private void Setup()
//        {
//            var services = new ServiceCollection();

//            services.AddDataProtection();

//            serviceProvider = services.BuildServiceProvider();

//            dataProtectionProvider = serviceProvider.GetService<IDataProtectionProvider>();
//            rawProtector = dataProtectionProvider.CreateProtector("sts.Licensing.Web.KeyPairManager");
//            persistentProtector = rawProtector as IPersistedDataProtector;

//            didSetup = true;
//        }

//        [Fact]
//        public void Can_Get_DataProtector_From_Services()
//        {
//            if (!didSetup) { Setup(); }

//            Assert.NotNull(serviceProvider);

//            Assert.NotNull(dataProtectionProvider);

//        }

//        [Fact]
//        public void Can_Cast_DataProtector_As_IPersistedDataProtector()
//        {
//            if (!didSetup) { Setup(); }

//            Assert.NotNull(rawProtector);

//            Assert.NotNull(persistentProtector);

//        }

//        [Fact]
//        public void Can_Round_Trip_Protection()
//        {
//            if (!didSetup) { Setup(); }

//            Assert.NotNull(persistentProtector);

//            string clearText = "Mozart was a fine violin player and composer";
//            string protectedText = persistentProtector.Protect(clearText);

//            string unprotectedText = persistentProtector.Unprotect(protectedText);

//            Assert.True(clearText == unprotectedText);

//        }

//        [Fact]
//        public void Can_Round_Trip_WithDangerous_Unprotect()
//        {
//            if (!didSetup) { Setup(); }

//            Assert.NotNull(persistentProtector);

//            string clearText = "Mozart was a fine violin player and composer";
//            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
//            string protectedText = persistentProtector.Protect(clearText);
//            byte[] protectedBytes = persistentProtector.Protect(clearBytes);
//            // this one fails so we can't expect to pass in string for Protect and then be
//            // able to DangerousUnprotect
//            //byte[] protectedBytes = Convert.FromBase64String(protectedText);

//            bool ignoreRevocation = true;
//            bool requiresMigration = false;
//            bool wasRevoked = false;
//            byte[] unprotectedBytes = persistentProtector.DangerousUnprotect(
//                protectedBytes,
//                ignoreRevocation,
//                out requiresMigration,
//                out wasRevoked);

//            string unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);

//            Assert.True(clearText == unprotectedText);

//        }

//        [Fact]
//        public void Can_Round_Trip_Persistent()
//        {
//            if (!didSetup) { Setup(); }

//            Assert.NotNull(persistentProtector);

//            string clearText = "Mozart was a fine violin player and composer";

//            string protectedText = persistentProtector.PersistentProtect(clearText);

//            bool requiresMigration = false;
//            bool wasRevoked = false;
//            string unprotectedText = persistentProtector.PersistentUnprotect(
//                protectedText,
//                out requiresMigration,
//                out wasRevoked);

//            Assert.True(clearText == unprotectedText);

//        }

//    }
//}
