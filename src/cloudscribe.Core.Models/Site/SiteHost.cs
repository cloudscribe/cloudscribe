// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2015-12-08
// 

using System;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Microsoft.Owin.Security.Cookies;
//using Owin;

namespace cloudscribe.Core.Models
{
    public interface ISiteHost
    {
        int HostId { get; set; }
        string HostName { get; set; }
        int SiteId { get; set; }
        Guid SiteGuid { get; set; }
        //bool IsDomain(IOwinContext context);

    }

    public class SiteHost : ISiteHost
    {
        public int HostId { get; set; }

        private string hostName = string.Empty;
        public string HostName
        {
            get { return hostName ?? string.Empty; }
            set { hostName = value; }
        }


        public int SiteId { get; set; } = -1;
        public Guid SiteGuid { get; set; } = Guid.Empty;
        //public bool IsDomain(IOwinContext context)
        //{
        //    return (context.Request.Host.Value == HostName);
        //}
    }
}
