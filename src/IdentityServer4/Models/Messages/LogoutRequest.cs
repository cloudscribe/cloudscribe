// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models the validated singout context.
    /// </summary>
    public class LogoutMessage
    {
        public LogoutMessage()
        {
        }

        public LogoutMessage(ValidatedEndSessionRequest request)
        {
            if (request != null)
            {
                Parameters = request.Raw;
                ClientId = request.Client?.ClientId;
                SessionId = request.SessionId;

                if (request.PostLogOutUri != null)
                {
                    PostLogoutRedirectUri = request.PostLogOutUri;
                    if (request.State != null)
                    {
                        PostLogoutRedirectUri = PostLogoutRedirectUri.AddQueryString(OidcConstants.EndSessionRequest.State, request.State);
                    }
                }
            }
        }

        public LogoutMessage(LogoutMessage message)
        {
            if (message != null)
            {
                ClientId = message.ClientId;
                PostLogoutRedirectUri = message.PostLogoutRedirectUri;
                SessionId = message.SessionId;
                Parameters = message.Parameters;
            }
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Gets or sets the post logout redirect URI.
        /// </summary>
        /// <value>
        /// The post logout redirect URI.
        /// </value>
        public string PostLogoutRedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the session identifier for the user at logout time.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets the entire parameter collection.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public NameValueCollection Parameters { get; } = new NameValueCollection();

        /// <summary>
        /// Gets or sets a value indicating whether the user should be prompted for signout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the signout prompt should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSignoutPrompt => ClientId.IsMissing();
    }

    /// <summary>
    /// Models the request from a client to sign the user out.
    /// </summary>
    public class LogoutRequest : LogoutMessage
    {
        public LogoutRequest(string iframeUrl, LogoutMessage message)
            : base(message)
        {
            SignOutIFrameUrl = iframeUrl;
        }

        /// <summary>
        /// Gets or sets the sign out iframe URL.
        /// </summary>
        /// <value>
        /// The sign out iframe URL.
        /// </value>
        public string SignOutIFrameUrl { get; set; }
    }
}