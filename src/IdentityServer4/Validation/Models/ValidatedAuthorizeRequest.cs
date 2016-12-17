// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Models a validated request to the authorize endpoint.
    /// </summary>
    public class ValidatedAuthorizeRequest : ValidatedRequest
    {
        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        /// <value>
        /// The type of the response.
        /// </value>
        public string ResponseType { get; set; }

        /// <summary>
        /// Gets or sets the response mode.
        /// </summary>
        /// <value>
        /// The response mode.
        /// </value>
        public string ResponseMode { get; set; }

        /// <summary>
        /// Gets or sets the grant type.
        /// </summary>
        /// <value>
        /// The the grant type.
        /// </value>
        public string GrantType { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the requested scopes.
        /// </summary>
        /// <value>
        /// The requested scopes.
        /// </value>
        public List<string> RequestedScopes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consent was shown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if consent was shown; otherwise, <c>false</c>.
        /// </value>
        public bool WasConsentShown { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the UI locales.
        /// </summary>
        /// <value>
        /// The UI locales.
        /// </value>
        public string UiLocales { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request was an OpenID Connect request.
        /// </summary>
        /// <value>
        /// <c>true</c> if the request was an OpenID Connect request; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenIdRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is API resource request.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is API resource request; otherwise, <c>false</c>.
        /// </value>
        public bool IsApiResourceRequest { get; set; }

        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the authentication context reference classes.
        /// </summary>
        /// <value>
        /// The authentication context reference classes.
        /// </value>
        public List<string> AuthenticationContextReferenceClasses { get; set; }

        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        /// <value>
        /// The display mode.
        /// </value>
        public string DisplayMode { get; set; }

        /// <summary>
        /// Gets or sets the prompt mode.
        /// </summary>
        /// <value>
        /// The prompt mode.
        /// </value>
        public string PromptMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum age.
        /// </summary>
        /// <value>
        /// The maximum age.
        /// </value>
        public int? MaxAge { get; set; }

        /// <summary>
        /// Gets or sets the login hint.
        /// </summary>
        /// <value>
        /// The login hint.
        /// </value>
        public string LoginHint { get; set; }

        /// <summary>
        /// Gets or sets the code challenge
        /// </summary>
        /// <value>
        /// The code challenge
        /// </value>
        public string CodeChallenge { get; set; }

        /// <summary>
        /// Gets or sets the code challenge method
        /// </summary>
        /// <value>
        /// The code challenge method
        /// </value>
        public string CodeChallengeMethod { get; set; }

        /// <summary>
        /// Gets a value indicating whether an access token was requested.
        /// </summary>
        /// <value>
        /// <c>true</c> if an access token was requested; otherwise, <c>false</c>.
        /// </value>
        public bool AccessTokenRequested => (ResponseType == OidcConstants.ResponseTypes.IdTokenToken ||
                                             ResponseType == OidcConstants.ResponseTypes.Code ||
                                             ResponseType == OidcConstants.ResponseTypes.CodeIdToken ||
                                             ResponseType == OidcConstants.ResponseTypes.CodeToken ||
                                             ResponseType == OidcConstants.ResponseTypes.CodeIdTokenToken);

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedAuthorizeRequest"/> class.
        /// </summary>
        public ValidatedAuthorizeRequest()
        {
            RequestedScopes = new List<string>();
            AuthenticationContextReferenceClasses = new List<string>();
        }
    }
}