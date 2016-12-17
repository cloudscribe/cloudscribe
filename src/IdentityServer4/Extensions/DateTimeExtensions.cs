// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;

namespace IdentityServer4.Extensions
{
    internal static class DateTimeExtensions
    {
        [DebuggerStepThrough]
        public static bool HasExceeded(this DateTime creationTime, int seconds)
        {
            return (DateTimeHelper.UtcNow > creationTime.AddSeconds(seconds));
        }

        [DebuggerStepThrough]
        public static int GetLifetimeInSeconds(this DateTime creationTime)
        {
            return ((int)(DateTimeHelper.UtcNow - creationTime).TotalSeconds);
        }

        [DebuggerStepThrough]
        public static bool HasExpired(this DateTime? expirationTime)
        {
            if (expirationTime.HasValue &&
                expirationTime < DateTimeHelper.UtcNow)
            {
                return true;
            }

            return false;
        }
    }
}