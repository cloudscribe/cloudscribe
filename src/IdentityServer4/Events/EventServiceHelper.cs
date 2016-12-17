// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Configuration;

namespace IdentityServer4.Events
{
    public class EventServiceHelper
    {
        private readonly IdentityServerOptions _options;
        private readonly IHttpContextAccessor _context;

        public EventServiceHelper(IdentityServerOptions options, IHttpContextAccessor context)
        {
            _options = options;
            _context = context;
        }

        public bool CanRaiseEvent<T>(Event<T> evt)
        {
            switch (evt.EventType)
            {
                case EventTypes.Failure:
                    return _options.EventsOptions.RaiseFailureEvents;
                case EventTypes.Information:
                    return _options.EventsOptions.RaiseInformationEvents;
                case EventTypes.Success:
                    return _options.EventsOptions.RaiseSuccessEvents;
                case EventTypes.Error:
                    return _options.EventsOptions.RaiseErrorEvents;
            }

            return false;
        }

        public virtual Event<T> PrepareEvent<T>(Event<T> evt)
        {
            if (evt == null) throw new ArgumentNullException("evt");

            evt.Context = new EventContext
            {
                ActivityId = _context.HttpContext.TraceIdentifier,
                TimeStamp = DateTimeHelper.UtcNow,
                ProcessId = Process.GetCurrentProcess().Id,
            };

            if (_context.HttpContext.Connection.RemoteIpAddress != null)
            {
                evt.Context.RemoteIpAddress = _context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                evt.Context.RemoteIpAddress = "unknown";
            }

            var principal = _context.HttpContext.User;
            if (principal != null && principal.Identity != null)
            {
                var subjectClaim = principal.FindFirst(JwtClaimTypes.Subject);
                if (subjectClaim != null)
                {
                    evt.Context.SubjectId = subjectClaim.Value;
                }
            }

            return evt;
        }
    }
}