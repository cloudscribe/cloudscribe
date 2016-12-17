// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    public class SecretParser
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ISecretParser> _parsers;

        public SecretParser(IEnumerable<ISecretParser> parsers, ILogger<SecretParser> logger)
        {
            _parsers = parsers;
            _logger = logger;
        }

        public async Task<ParsedSecret> ParseAsync(HttpContext context)
        {
            // see if a registered parser finds a secret on the request
            ParsedSecret bestSecret = null;
            foreach (var parser in _parsers)
            {
                ParsedSecret parsedSecret = null;
                parsedSecret = await parser.ParseAsync(context);
                if (parsedSecret != null)
                {
                    _logger.LogDebug("Parser found secret: {type}", parser.GetType().Name);

                    bestSecret = parsedSecret;

                    if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.NoSecret)
                    {
                        break;
                    }
                }
            }

            if (bestSecret != null)
            {
                _logger.LogDebug("Secret id found: {id}", bestSecret.Id);
                return bestSecret;
            }

            _logger.LogDebug("Parser found no secret");
            return null;
        }

        public IEnumerable<string> GetAvailableAuthenticationMethods()
        {
            return _parsers.Select(p => p.AuthenticationMethod);
        }
    }
}