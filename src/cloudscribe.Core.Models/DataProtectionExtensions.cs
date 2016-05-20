// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-14
// Last Modified:			2016-05-17
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace cloudscribe.Core.Models
{
    public static class DataProtectionExtensions
    {
        public static string PersistentUnprotect(
            this IPersistedDataProtector dp,
            string protectedData,
            out bool requiresMigration,
            out bool wasRevoked)
        {
            bool ignoreRevocation = true;
            byte[] protectedBytes = Convert.FromBase64String(protectedData);
            byte[] unprotectedBytes = dp.DangerousUnprotect(protectedBytes, ignoreRevocation, out requiresMigration, out wasRevoked);

            return Encoding.UTF8.GetString(unprotectedBytes);
        }

        public static string PersistentProtect(
            this IPersistedDataProtector dp,
            string clearText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] protectedBytes = dp.Protect(clearBytes);

            string result = Convert.ToBase64String(protectedBytes);
            return result;

        }

    }

}
