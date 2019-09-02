// Licensed under the Apache License, Version 2.0
//	Author:                 Joe Audette
//  Created:			    2016-06-06
//	Last Modified:		    2019-09-01
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;

using Microsoft.AspNetCore.Http;
using cloudscribe.Multitenancy;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteTimeZoneIdResolver : cloudscribe.DateTimeUtils.ITimeZoneIdResolver
    {
        public SiteTimeZoneIdResolver(
            IHttpContextAccessor contextAccessor,
            ITenantResolver<SiteContext> siteResolver,
            SiteUserManager<SiteUser> userManager
            )
        {
            _contextAccessor = contextAccessor;
            _siteResolver = siteResolver;
            _userManager = userManager;
        }

        private IHttpContextAccessor _contextAccessor;
        private ITenantResolver<SiteContext> _siteResolver;
        private SiteUserManager<SiteUser> _userManager;

        public async Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var context = _contextAccessor.HttpContext;
            if (context.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByIdAsync(context.User.GetUserId());
                if ((user != null) && (!string.IsNullOrEmpty(user.TimeZoneId)))
                {
                    return user.TimeZoneId;
                }
            }

            return await GetSiteTimeZoneId(cancellationToken);
        }

        public Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tenant = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (tenant != null)
            {
                if (!string.IsNullOrEmpty(tenant.TimeZoneId))
                    return Task.FromResult(tenant.TimeZoneId);
            }

            return Task.FromResult("America/New_York"); //default
        }

    }

    //TODO: deprecate
    //[Obsolete("Please use SiteTimeZoneIdResolver instead")]
    //public class RequestTimeZoneIdResolver : cloudscribe.Web.Common.ITimeZoneIdResolver
    //{
    //    public RequestTimeZoneIdResolver(
    //        IHttpContextAccessor contextAccessor,
    //        ITenantResolver<SiteContext> siteResolver,
    //        SiteUserManager<SiteUser> userManager
    //        )
    //    {
    //        _contextAccessor = contextAccessor;
    //        _siteResolver = siteResolver;
    //        _userManager = userManager;
    //    }

    //    private IHttpContextAccessor _contextAccessor;
    //    private ITenantResolver<SiteContext> _siteResolver;
    //    private SiteUserManager<SiteUser> _userManager;

    //    public async Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();

    //        var context = _contextAccessor.HttpContext;
    //        if (context.User.Identity.IsAuthenticated)
    //        {
    //            var user = await _userManager.FindByIdAsync(context.User.GetUserId());
    //            if ((user != null) && (!string.IsNullOrEmpty(user.TimeZoneId)))
    //            {
    //                return user.TimeZoneId;
    //            }
    //        }

    //        return await GetSiteTimeZoneId(cancellationToken);
    //    }

    //    public Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();

    //        var tenant = _contextAccessor.HttpContext.GetTenant<SiteContext>();
    //        if (tenant != null)
    //        {
    //            if (!string.IsNullOrEmpty(tenant.TimeZoneId))
    //                return Task.FromResult(tenant.TimeZoneId);
    //        }

    //        return Task.FromResult("America/New_York"); //default
    //    }

    //}
}
