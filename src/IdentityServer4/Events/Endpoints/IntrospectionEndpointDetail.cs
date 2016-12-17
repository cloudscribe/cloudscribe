// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Events
{
    public class IntrospectionEndpointDetail
    {
        public string EndpointName { get; set; } = EventConstants.EndpointNames.Introspection;
        public string Token { get; set; }
        public string ApiName { get; set; }
        public string TokenStatus { get; set; }
    }
}