// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2015-11-21
// 

using System.Collections.Generic;
using cloudscribe.Core.Models.Geography;

namespace cloudscribe.Core.Models
{
    public static class ModelExtensions
    {

        public static bool IsDeletable(this ISiteRole role, string undeletableRolesSemiColonSeparated)
        {
            List<string> rolesThatCannotBeDelete = undeletableRolesSemiColonSeparated.SplitOnChar(';');
            return role.IsDeletable(rolesThatCannotBeDelete);
        }

        public static bool IsDeletable(this ISiteRole role, List<string> rolesThatCannotBeDeleted)
        {
            if (role.RoleName == "Admins") { return false; }
            if (role.RoleName == "Content Administrators") { return false; }
            if (role.RoleName == "Authenticated Users") { return false; }
            if (role.RoleName == "Role Admins") { return false; }

            if (rolesThatCannotBeDeleted != null)
            {
                foreach (string roleName in rolesThatCannotBeDeleted)
                {
                    if (role.RoleName == roleName) { return false; }
                    if (role.DisplayName == roleName) { return false; }
                }
            }

            return true;
        }

        public static GeoCountry ToGeoCountry(this IGeoCountry igeo)
        {
            GeoCountry country = new GeoCountry();
            country.Guid = igeo.Guid;
            country.ISOCode2 = igeo.ISOCode2;
            country.ISOCode3 = igeo.ISOCode3;
            country.Name = igeo.Name;

            return country;
        }

        public static GeoZone ToGeoZone(this IGeoZone igeo)
        {
            GeoZone state = new GeoZone();
            state.Guid = igeo.Guid;
            state.CountryGuid = igeo.CountryGuid;
            state.Code = igeo.Code;
            state.Name = igeo.Name;

            return state;
        }
    }
}
