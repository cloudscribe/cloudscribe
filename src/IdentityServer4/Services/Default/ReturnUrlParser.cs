// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Services.Default
{
    public class ReturnUrlParser
    {
        private readonly IEnumerable<IReturnUrlParser> _parsers;

        public ReturnUrlParser(IEnumerable<IReturnUrlParser> parsers)
        {
            _parsers = parsers;
        }

        public virtual async Task<AuthorizationRequest> ParseAsync(string returnUrl)
        {
            foreach (var parser in _parsers)
            {
                if (parser.IsValidReturnUrl(returnUrl))
                {
                    var result = await parser.ParseAsync(returnUrl);
                    return result;
                }
            }

            return null;            
        }

        public virtual bool IsValidReturnUrl(string returnUrl)
        {
            foreach (var parser in _parsers)
            {
                if (parser.IsValidReturnUrl(returnUrl))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
