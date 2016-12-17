// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Services.Default
{
    /// <summary>
    /// Default consent service
    /// </summary>
    public class DefaultConsentService : IConsentService
    {
        /// <summary>
        /// The user consent store
        /// </summary>
        protected readonly IUserConsentStore _userConsentStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultConsentService" /> class.
        /// </summary>
        /// <param name="grants">The grants.</param>
        /// <exception cref="System.ArgumentNullException">store</exception>
        public DefaultConsentService(IUserConsentStore userConsentStore)
        {
            _userConsentStore = userConsentStore;
        }

        /// <summary>
        /// Checks if consent is required.
        /// </summary>
        /// <param name="subject">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns>
        /// Boolean if consent is required.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// client
        /// or
        /// subject
        /// </exception>
        public virtual async Task<bool> RequiresConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<string> scopes)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            if (!client.RequireConsent)
            {
                return false;
            }

            // TODO: validate that this is a correct statement
            if (!client.AllowRememberConsent)
            {
                return true;
            }

            if (scopes == null || !scopes.Any())
            {
                return false;
            }

            // we always require consent for offline access if
            // the client has not disabled RequireConsent 
            if (scopes.Contains(IdentityServerConstants.StandardScopes.OfflineAccess))
            {
                return true;
            }
            
            var consent = await _userConsentStore.GetUserConsentAsync(subject.GetSubjectId(), client.ClientId);
            if (consent != null && consent.Scopes != null)
            {
                var intersect = scopes.Intersect(consent.Scopes);
                return !(scopes.Count() == intersect.Count());
            }

            return true;
        }

        /// <summary>
        /// Updates the consent asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// client
        /// or
        /// subject
        /// </exception>
        public virtual async Task UpdateConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<string> scopes)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            if (client.AllowRememberConsent)
            {
                var subjectId = subject.GetSubjectId();
                var clientId = client.ClientId;

                if (scopes != null && scopes.Any())
                {
                    var consent = new Consent
                    {
                        SubjectId = subjectId,
                        ClientId = clientId,
                        Scopes = scopes
                    };
                    await _userConsentStore.StoreUserConsentAsync(consent);
                }
                else
                {
                    await _userConsentStore.RemoveUserConsentAsync(subjectId, clientId);
                }
            }
        }
    }
}