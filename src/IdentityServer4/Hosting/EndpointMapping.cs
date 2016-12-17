// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4.Hosting
{
    public class EndpointMapping
    {
        public EndpointName Endpoint { get; set; }
        public Type Handler { get; set; }
    }
}
