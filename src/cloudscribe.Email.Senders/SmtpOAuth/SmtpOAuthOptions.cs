// Copyright (c) Idox Software Ltd. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts
// Created:					2023-04-19
// Last Modified:			2023-04-19
//


using System;

namespace cloudscribe.Email.SmtpOAuth
{
    public class SmtpOAuthOptions
    {
        public string SiteId { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; } = false;

        public string User { get; set; } = string.Empty; //SMTP username

        public string AuthorizeEndpoint { get; set; } = string.Empty;
        public string TokenEndpoint { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string ScopesCsv { get; set; } = string.Empty;
        public string SecureToken { get; set; } = string.Empty;
        public DateTime? TokenExpiresUtc { get; set; }

        public string HtmlBodyDefaultEncoding { get; set; }
        public string PlainTextBodyDefaultEncoding { get; set; }
        public string DefaultEmailFromAddress { get; set; } = string.Empty;
        public string DefaultEmailFromAlias { get; set; } = string.Empty;
    }
}
