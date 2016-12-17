// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Hosting
{
    public enum EndpointName
    {
        Authorize,
        Token,
        Discovery,
        Introspection,
        Revocation,
        EndSession,
        CheckSession,
        UserInfo
    }
}
