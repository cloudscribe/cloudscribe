// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-01-31
// Last Modified:		    2018-06-24
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
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
                //var connection = context.Connection;
                //if (connection == null) return;
                //var ip = connection.RemoteIpAddress;
                //if (ip == null) return;
                //var ipv4 = ip.MapToIPv4();

                //string ipv4Address = ipv4.ToString();
                ////Connection.RemoteIpAddress.MapToIPv4().ToLong()

                //if (string.IsNullOrEmpty(ipv4Address)) { return; }
                long ip4aslong = 0;
                //if (ip4aslong == 0) { return; }
                var ipAddress = GetIpAddress();

                var ipvAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress;
                if(ipvAddress != null)
                {
                    ip4aslong = ipvAddress.MapToIPv4().ToLong();
                }
                if (ip4aslong == 0) { return; }

                var userLocation = await _queries.FetchLocationByUserAndIpv4Address(siteId, userGuid, ip4aslong, CancellationToken.None);
                if (userLocation == null)
                {
                    userLocation = new UserLocation
                    {
                        SiteId = siteId,
                        UserId = userGuid,
                        IpAddress = ipAddress,
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

        public string GetIpAddress(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                ip = GetHeaderValueAs<string>("X-Forwarded-For").TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList().FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _contextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
            {
                var ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress;
                ip = ipAddress.MapToIPv4().ToString();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");
            }

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_contextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                {
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
                }
            }
            return default(T);
        }

    }
}
