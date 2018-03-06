// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-01
// Last Modified:			2016-11-14
// 


namespace cloudscribe.Email.Smtp
{
    public class SmtpOptions
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSsl { get; set; } = false;
        public bool RequiresAuthentication { get; set; } = false;
        public string HtmlBodyDefaultEncoding { get; set; }
        public string PlainTextBodyDefaultEncoding { get; set; }

        public string DefaultEmailFromAddress { get; set; } = string.Empty;

        public string DefaultEmailFromAlias { get; set; } = string.Empty;
    }
}
