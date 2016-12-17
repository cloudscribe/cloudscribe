// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Validation
{
    public class IntrospectionRequestValidationResult : ValidationResult
    {
        public bool IsActive { get; set; }
        public IntrospectionRequestValidationFailureReason FailureReason { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public string Token { get; set; }
    }

    public enum IntrospectionRequestValidationFailureReason
    {
        None,
        MissingToken,
        InvalidToken,
        InvalidScope
    }
}