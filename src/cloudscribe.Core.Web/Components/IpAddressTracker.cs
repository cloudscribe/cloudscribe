// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-01-31
// Last Modified:		    2016-02-03
// 


using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Components
{
    public class IpAddressTracker
    {
        public IpAddressTracker(
            IUserRepository userRepository,
            ILogger<IpAddressTracker> logger,
            IHttpContextAccessor contextAccessor)
        {
            userRepo = userRepository;
            log = logger;
            context = contextAccessor?.HttpContext;
        }

        private IUserRepository userRepo;
        private ILogger log;

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;

        public async Task<bool> TackUserIpAddress(Guid siteGuid, Guid userGuid)
        {
           
            if (context == null) { return false; }
            if( userGuid == Guid.Empty) { return false; }

            try
            {
                IPAddress ipv4 = context.Connection.RemoteIpAddress.MapToIPv4();

                string ipv4Address = ipv4.ToString();
                //Connection.RemoteIpAddress.MapToIPv4().ToLong()

                if (string.IsNullOrEmpty(ipv4Address)) { return false; }
                long ip4aslong = ipv4.ToLong();
                if (ip4aslong == 0) { return false; }

                //string hostName = context.Connection. 
                //doesnt seem a good way to get client host name but that doesn't often have any meaning value anyway

                var userLocation = await userRepo.FetchLocationByUserAndIpv4Address(userGuid, ip4aslong, CancellationToken);
                if (userLocation == null)
                {
                    userLocation = new UserLocation();
                    userLocation.SiteGuid = siteGuid;
                    userLocation.UserGuid = userGuid;
                    userLocation.IpAddress = ipv4Address;
                    userLocation.IpAddressLong = ip4aslong;
                    userLocation.FirstCaptureUtc = DateTime.UtcNow;
                    userLocation.LastCaptureUtc = userLocation.FirstCaptureUtc;
                    userLocation.CaptureCount = 1;
                    bool result = await userRepo.AddUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);

                }
                else
                {
                    userLocation.LastCaptureUtc = DateTime.UtcNow;
                    userLocation.CaptureCount += 1;
                    bool result = await userRepo.UpdateUserLocation(userLocation, CancellationToken.None)
                        .ConfigureAwait(false);
                }


                return true;
            }
            catch (Exception ex)
            {
                log.LogError("error in iptracker", ex);

            }

            return false;
        }
    }
}
