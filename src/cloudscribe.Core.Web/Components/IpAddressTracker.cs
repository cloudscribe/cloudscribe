// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-01-31
// Last Modified:		    2016-08-03
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            commands = userCommands;
            queries = userQueries;
            log = logger;
            context = contextAccessor?.HttpContext;
        }

        private IUserCommands commands;
        private IUserQueries queries;
        private ILogger log;

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;

        public async Task TackUserIpAddress(Guid siteId, Guid userGuid)
        {
           
            if (context == null) { return; }
            if( userGuid == Guid.Empty) { return; }

            try
            {
                var connection = context.Connection;
                if (connection == null) return;
                var ip = connection.RemoteIpAddress;
                if (ip == null) return;
                var ipv4 = ip.MapToIPv4();

                string ipv4Address = ipv4.ToString();
                //Connection.RemoteIpAddress.MapToIPv4().ToLong()

                if (string.IsNullOrEmpty(ipv4Address)) { return; }
                long ip4aslong = ipv4.ToLong();
                if (ip4aslong == 0) { return; }

                //string hostName = context.Connection. 
                //doesnt seem a good way to get client host name but that doesn't often have any meaning value anyway

                var userLocation = await queries.FetchLocationByUserAndIpv4Address(siteId, userGuid, ip4aslong, CancellationToken);
                if (userLocation == null)
                {
                    userLocation = new UserLocation();
                    userLocation.SiteId = siteId;
                    userLocation.UserId = userGuid;
                    userLocation.IpAddress = ipv4Address;
                    userLocation.IpAddressLong = ip4aslong;
                    userLocation.FirstCaptureUtc = DateTime.UtcNow;
                    userLocation.LastCaptureUtc = userLocation.FirstCaptureUtc;
                    userLocation.CaptureCount = 1;
                    await commands.AddUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);

                }
                else
                {
                    userLocation.LastCaptureUtc = DateTime.UtcNow;
                    userLocation.CaptureCount += 1;
                    await commands.UpdateUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);
                }


                
            }
            catch (Exception ex)
            {
                log.LogError("error in iptracker", ex);

            }

            
        }
    }
}
