// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    public class ScopeValidator
    {
        private readonly ILogger _logger;
        private readonly IResourceStore _store;

        public bool ContainsOpenIdScopes => GrantedResources.IdentityResources.Any();
        public bool ContainsApiResourceScopes => GrantedResources.ApiResources.Any();
        public bool ContainsOfflineAccessScope => GrantedResources.OfflineAccess;

        public Resources RequestedResources { get; internal set; } = new Resources();
        public Resources GrantedResources { get; internal set; } = new Resources();

        public ScopeValidator(IResourceStore store, ILogger<ScopeValidator> logger)
        {
            _logger = logger;
            _store = store;
        }

        public bool ValidateRequiredScopes(IEnumerable<string> consentedScopes)
        {
            var identity = RequestedResources.IdentityResources.Where(x => x.Required).Select(x=>x.Name);
            var apiQuery = from api in RequestedResources.ApiResources
                           where api.Scopes != null
                           from scope in api.Scopes
                           where scope.Required
                           select scope.Name;

            var requiredScopes = identity.Union(apiQuery);
            return requiredScopes.All(x => consentedScopes.Contains(x));
        }

        public void SetConsentedScopes(IEnumerable<string> consentedScopes)
        {
            consentedScopes = consentedScopes ?? Enumerable.Empty<string>();

            var offline = consentedScopes.Contains(IdentityServerConstants.StandardScopes.OfflineAccess);
            if (offline)
            {
                consentedScopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess);
            }

            var identityToKeep = GrantedResources.IdentityResources.Where(x => x.Required || consentedScopes.Contains(x.Name));
            var apisToKeep = from api in GrantedResources.ApiResources
                             where api.Scopes != null
                             let scopesToKeep = (from scope in api.Scopes
                                                 where scope.Required == true || consentedScopes.Contains(scope.Name)
                                                 select scope)
                             where scopesToKeep.Any()
                             select api.CloneWithScopes(scopesToKeep);

            GrantedResources = new Resources(identityToKeep, apisToKeep)
            {
                OfflineAccess = offline
            };
        }

        public async Task<bool> AreScopesValidAsync(IEnumerable<string> requestedScopes)
        {
            if (requestedScopes.Contains(IdentityServerConstants.StandardScopes.OfflineAccess))
            {
                GrantedResources.OfflineAccess = true;
                requestedScopes = requestedScopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess).ToArray();
            }

            var resources = await _store.FindResourcesByScopeAsync(requestedScopes);

            foreach (var requestedScope in requestedScopes)
            {
                var identity = resources.IdentityResources.FirstOrDefault(x => x.Name == requestedScope);
                if (identity != null)
                {
                    if (identity.Enabled == false)
                    {
                        _logger.LogError("Scope disabled: {requestedScope}", requestedScope);
                        return false;
                    }

                    GrantedResources.IdentityResources.Add(identity);
                }
                else
                {
                    var api = resources.FindApiResourceByScope(requestedScope);
                    if (api == null)
                    {
                        _logger.LogError("Invalid scope: {requestedScope}", requestedScope);
                        return false;
                    }

                    if (api.Enabled == false)
                    {
                        _logger.LogError("API {api} that conatins scope is disabled: {requestedScope}", api.Name, requestedScope);
                        return false;
                    }

                    var scope = api.FindApiScope(requestedScope);

                    if (scope == null)
                    {
                        _logger.LogError("Invalid scope: {requestedScope}", requestedScope);
                        return false;
                    }

                    // see if we already have this API in our list
                    var existingApi = GrantedResources.ApiResources.FirstOrDefault(x => x.Name == api.Name);
                    if (existingApi != null)
                    {
                        existingApi.Scopes.Add(scope);
                    }
                    else
                    {
                        GrantedResources.ApiResources.Add(api.CloneWithScopes(new Scope[] { scope }));
                    }
                }
            }

            RequestedResources = new Resources(GrantedResources.IdentityResources, GrantedResources.ApiResources)
            {
                OfflineAccess = GrantedResources.OfflineAccess
            };

            return true;
        }

        public async Task<bool> AreScopesAllowedAsync(Client client, IEnumerable<string> requestedScopes)
        {
            if (requestedScopes.Contains(IdentityServerConstants.StandardScopes.OfflineAccess))
            {
                if (client.AllowOfflineAccess == false)
                {
                    _logger.LogError("offline_access is not allowed for this client: {client}", client.ClientId);
                    return false;
                }
                requestedScopes = requestedScopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess).ToArray();
            }

            var resources = await _store.FindEnabledResourcesByScopeAsync(requestedScopes);

            foreach (var scope in requestedScopes)
            {
                var identity = resources.IdentityResources.FirstOrDefault(x => x.Name == scope);
                if (identity != null)
                {
                    if (!client.AllowedScopes.Contains(scope))
                    {
                        _logger.LogError("Requested scope not allowed: {scope}", scope);
                        return false;
                    }
                }
                else
                {
                    var api = resources.FindApiScope(scope);
                    if (api == null || !client.AllowedScopes.Contains(scope))
                    {
                        _logger.LogError("Requested scope not allowed: {scope}", scope);
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsResponseTypeValid(string responseType)
        {
            var requirement = Constants.ResponseTypeToScopeRequirement[responseType];

            switch (requirement)
            {
                case Constants.ScopeRequirement.Identity:
                    if (!ContainsOpenIdScopes)
                    {
                        _logger.LogError("Requests for id_token response type must include identity scopes");
                        return false;
                    }
                    break;
                case Constants.ScopeRequirement.IdentityOnly:
                    if (!ContainsOpenIdScopes || ContainsApiResourceScopes)
                    {
                        _logger.LogError("Requests for id_token response type only must not include resource scopes");
                        return false;
                    }
                    break;
                case Constants.ScopeRequirement.ResourceOnly:
                    if (ContainsOpenIdScopes || !ContainsApiResourceScopes)
                    {
                        _logger.LogError("Requests for token response type only must include resource scopes, but no identity scopes.");
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}