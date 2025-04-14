﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-04
// Last Modified:			2015-09-05


namespace cloudscribe.Core.Web.Components
{
    public class UIOptions
    {
        public UIOptions()
        { }

        public int DefaultPageSize_SiteList { get; set; } = 10;

        public bool AllowDeleteChildSites { get; set; } = false;
        public bool AllowAdminsToChangeUserPasswords { get; set; } = true;

        public int DefaultPageSize_CountryList { get; set; } = 10;
        public int DefaultPageSize_StateList { get; set; } = 10;
        public int DefaultPageSize_RoleList { get; set; } = 10;
        public int DefaultPageSize_RoleMemberList { get; set; } = 10;
        public int DefaultPageSize_UserList { get; set; } = 10;
        public int DefaultPageSize_LogView { get; set; } = 10;

        // these props should be deprecated, keeping for backward compat with bootstrap3 themes
        public string IconEmail { get; set; } = "glyphicon glyphicon-envelope";
        public string IconUsername { get; set; } = "glyphicon glyphicon-circle-arrow-right";
        public string IconPassword { get; set; } = "glyphicon glyphicon-lock";
        public string IconFirstName { get; set; } = "glyphicon glyphicon-user";
        public string IconLastName { get; set; } = "glyphicon glyphicon-user";
        public string IconDateOfBirth { get; set; } = "glyphicon glyphicon-calendar";

    }
}
