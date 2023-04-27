// Copyright (c) Idox Software Ltd. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts
// Created:					2023-04-19
// Last Modified:			2023-04-19
//

using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Email.SmtpOAuth
{
    public class ConfigSmtpOAuthOptionsProvider : ISmtpOAuthOptionsProvider
    {
        public ConfigSmtpOAuthOptionsProvider(IOptions<SmtpOAuthOptions> smtpOAuthOptionsAccessor)
        {
            _smtpOAuthSettings = smtpOAuthOptionsAccessor.Value;
        }

        private SmtpOAuthOptions _smtpOAuthSettings;
        public virtual Task<SmtpOAuthOptions> GetSmtpOAuthOptions(string lookupKey = null)
        {
            return Task.FromResult(_smtpOAuthSettings);
        }
    }
}
