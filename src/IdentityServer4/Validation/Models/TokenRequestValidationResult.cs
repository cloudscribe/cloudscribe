// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validation result for token requests
    /// </summary>
    public class TokenRequestValidationResult : ValidationResult
    {
        public TokenRequestValidationResult(ValidatedTokenRequest validatedRequest, Dictionary<string, object> customResponse = null)
        {
            IsError = false;

            ValidatedRequest = validatedRequest;
            CustomResponse = customResponse;
        }

        public TokenRequestValidationResult(string error, string errorDescription = null, Dictionary<string, object> customResponse = null)
        {
            IsError = true;

            Error = error;
            ErrorDescription = errorDescription;
            CustomResponse = customResponse;
        }

        public ValidatedTokenRequest ValidatedRequest { get; }
        public Dictionary<string, object> CustomResponse { get; set; }
    }
}