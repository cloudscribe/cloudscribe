// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4
{
    internal static class DateTimeHelper
    {
        internal static Func<DateTime> UtcNowFunc = () => DateTime.UtcNow;

        internal static DateTime UtcNow => UtcNowFunc();
    }
}
