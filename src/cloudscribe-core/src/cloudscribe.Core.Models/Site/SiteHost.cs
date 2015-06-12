// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2014-08-17
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
        public string HostName { get; set; }
        public int SiteId { get; set; }
        public Guid SiteGuid { get; set; }
        //public bool IsDomain(IOwinContext context)
        //{
        //    return (context.Request.Host.Value == HostName);
        //}
    }
}
