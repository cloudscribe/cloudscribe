// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel;

#pragma warning disable 1591

namespace IdentityServer4.Validation
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum BearerTokenUsageType
    {
        AuthorizationHeader = 0,
        PostBody = 1,
        QueryString = 2
    }
}