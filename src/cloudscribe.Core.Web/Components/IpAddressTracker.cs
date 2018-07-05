// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-01-31
// Last Modified:		    2018-06-24
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


//https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1
//https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1#forwarded-headers-middleware-options
//https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.1&tabs=aspnetcore2x

namespace cloudscribe.Core.Web.Components
{
    public class IpAddressTracker
    {
        public IpAddressTracker(
            IUserCommands userCommands,
            IUserQueries userQueries,
            ILogger<IpAddressTracker> logger,
            IHttpContextAccessor contextAccessor)
        {
            _commands = userCommands;
            _queries = userQueries;
            _log = logger;
            _contextAccessor = contextAccessor;
        }

        private IUserCommands _commands;
        private IUserQueries _queries;
        private ILogger _log;
        private IHttpContextAccessor _contextAccessor;

        //private readonly HttpContext context;
        //private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;

        public async Task TackUserIpAddress(Guid siteId, Guid userGuid)
        {
           
            if (_contextAccessor == null) { return; }
            if( userGuid == Guid.Empty) { return; }

            try
            {
                if ((_contextAccessor == null) || (_contextAccessor.HttpContext == null)) { return; }

                var connection = _contextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>();
                if(connection == null) { return; }
                
                var ip = connection.RemoteIpAddress;
                if (ip == null) return;
                var ipv4 = ip.MapToIPv4();
                string ipv4Address = ipv4.ToString();
                if (string.IsNullOrEmpty(ipv4Address)) { return; }
                long ip4aslong = ipv4.ToLong();
                if (ip4aslong == 0) { return; }
                
                var userLocation = await _queries.FetchLocationByUserAndIpv4Address(siteId, userGuid, ip4aslong, CancellationToken.None);
                if (userLocation == null)
                {
                    userLocation = new UserLocation
                    {
                        SiteId = siteId,
                        UserId = userGuid,
                        IpAddress = ipv4Address,
                        IpAddressLong = ip4aslong,
                        FirstCaptureUtc = DateTime.UtcNow
                    };
                    userLocation.LastCaptureUtc = userLocation.FirstCaptureUtc;
                    userLocation.CaptureCount = 1;
                    await _commands.AddUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);

                }
                else
                {
                    userLocation.LastCaptureUtc = DateTime.UtcNow;
                    userLocation.CaptureCount += 1;
                    await _commands.UpdateUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);
                }


                
            }
            catch (Exception ex)
            {
                _log.LogError("error in iptracker: " + ex.Message + " stacktrace: " + ex.StackTrace);

            }

            
        }

        
    }
}
