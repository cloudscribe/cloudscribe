// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validation result for end session callback requests.
    /// </summary>
    /// <seealso cref="IdentityServer4.Validation.ValidationResult" />
    public class EndSessionCallbackValidationResult : ValidationResult
    {
        public IEnumerable<string> ClientLogoutUrls { get; internal set; }

        // var logoutId = context.Request.Query[_options.UserInteractionOptions.LogoutIdParameter].FirstOrDefault();
        public string LogoutId { get; internal set; }

        public string SessionId { get; set; }
    }
}