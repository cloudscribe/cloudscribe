// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Validation
{
    public class BearerTokenUsageValidationResult
    {
        public bool TokenFound { get; set; }
        public string Token { get; set; }
        public BearerTokenUsageType UsageType { get; set; }
    }
}