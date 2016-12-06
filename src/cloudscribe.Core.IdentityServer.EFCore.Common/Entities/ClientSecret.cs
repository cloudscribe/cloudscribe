// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class ClientSecret : Secret
    {
        public Client Client { get; set; }
    }
}