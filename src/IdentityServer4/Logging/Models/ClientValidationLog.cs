// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Logging
{
    internal class ClientValidationLog
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCredentialType { get; set; }

        public override string ToString()
        {
            return LogSerializer.Serialize(this);
        }
    }
}