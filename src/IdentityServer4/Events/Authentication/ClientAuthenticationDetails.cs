// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Events
{
    public class ClientAuthenticationDetails
    {
        public string ClientId { get; set; }
        public string ClientType { get; set; }
    }
}