// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;

namespace IdentityServer4.Models
{
    internal static class ClientExtensions
    {
        public static bool IsImplicitOnly(this Client client)
        {
            return client != null &&
                client.AllowedGrantTypes != null &&
                client.AllowedGrantTypes.Count() == 1 &&
                client.AllowedGrantTypes.First() == GrantType.Implicit;
        }
    }
}