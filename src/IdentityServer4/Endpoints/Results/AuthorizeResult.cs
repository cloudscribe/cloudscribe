// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using IdentityServer4.Services;
using IdentityServer4.Configuration;
using IdentityServer4.Stores;

namespace IdentityServer4.Endpoints.Results
{
    class AuthorizeResult : IEndpointResult
    {
        public AuthorizeResponse Response { get; }

        public AuthorizeResult(AuthorizeResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            Response = response;
        }

        internal AuthorizeResult(
            AuthorizeResponse response,
            IdentityServerOptions options,
            IClientSessionService clientSession,
            IMessageStore<ErrorMessage> errorMessageStore)
            : this(response)
        {
            _options = options;
            _clientSession = clientSession;
            _errorMessageStore = errorMessageStore;
        }

        private IdentityServerOptions _options;
        private IClientSessionService _clientSession;
        private IMessageStore<ErrorMessage> _errorMessageStore;

        void Init(HttpContext context)
        {
            _options = _options ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
            _clientSession = _clientSession ?? context.RequestServices.GetRequiredService<IClientSessionService>();
            _errorMessageStore = _errorMessageStore ?? context.RequestServices.GetRequiredService<IMessageStore<ErrorMessage>>();
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            Init(context);

            if (Response.IsError)
            {
                await ProcessErrorAsync(context);
            }
            else
            {
                await ProcessResponseAsync(context);
            }
        }

        async Task ProcessErrorAsync(HttpContext context)
        {
            // these are the conditions where we can send a response 
            // back directly to the client, otherwise we're only showing the error UI
            var isPromptNoneError = Response.Error == OidcConstants.AuthorizeErrors.AccountSelectionRequired ||
                Response.Error == OidcConstants.AuthorizeErrors.LoginRequired ||
                Response.Error == OidcConstants.AuthorizeErrors.ConsentRequired ||
                Response.Error == OidcConstants.AuthorizeErrors.InteractionRequired;

            if (Response.Error == OidcConstants.AuthorizeErrors.AccessDenied ||
                (isPromptNoneError && Response.Request?.PromptMode == OidcConstants.PromptModes.None)
            )
            {
                // this scenario we can return back to the client
                await ProcessResponseAsync(context);
            }
            else
            {
                // we now know we must show error page
                await RedirectToErrorPageAsync(context);
            }
        }

        protected async Task ProcessResponseAsync(HttpContext context)
        {
            if (!Response.IsError)
            {
                // success response -- track client authorization for sign-out
                //_logger.LogDebug("Adding client {0} to client list cookie for subject {1}", request.ClientId, request.Subject.GetSubjectId());
                await _clientSession.AddClientIdAsync(Response.Request.ClientId);
            }

            await RenderAuthorizeResponseAsync(context);
        }

        private async Task RenderAuthorizeResponseAsync(HttpContext context)
        {
            if (Response.Request.ResponseMode == OidcConstants.ResponseModes.Query ||
                Response.Request.ResponseMode == OidcConstants.ResponseModes.Fragment)
            {
                context.Response.SetNoCache();
                context.Response.Redirect(BuildRedirectUri());
            }
            else if (Response.Request.ResponseMode == OidcConstants.ResponseModes.FormPost)
            {
                context.Response.SetNoCache();
                AddCspHeaders(context);
                await context.Response.WriteHtmlAsync(GetFormPostHtml());
            }
            else
            {
                //_logger.LogError("Unsupported response mode.");
                throw new InvalidOperationException("Unsupported response mode");
            }
        }

        private void AddCspHeaders(HttpContext context)
        {
            var formOrigin = Response.Request.RedirectUri.GetOrigin();
            // 'unsafe-inline' for edge
            var value = $"default-src 'none'; frame-ancestors {formOrigin}; form-action {formOrigin}; script-src 'unsafe-inline' 'sha256-VuNUSJ59bpCpw62HM2JG/hCyGiqoPN3NqGvNXQPU+rY=';";

            if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                context.Response.Headers.Add("Content-Security-Policy", value);
            }

            if (!context.Response.Headers.ContainsKey("X-Content-Security-Policy"))
            {
                context.Response.Headers.Add("X-Content-Security-Policy", value);
            }
        }

        string BuildRedirectUri()
        {
            var uri = Response.RedirectUri;
            var query = Response.ToNameValueCollection().ToQueryString();

            if (Response.Request.ResponseMode == OidcConstants.ResponseModes.Query)
            {
                uri = uri.AddQueryString(query);
            }
            else
            {
                uri = uri.AddHashFragment(query);
            }

            if (Response.IsError && !uri.Contains("#"))
            {
                // https://tools.ietf.org/html/draft-bradley-oauth-open-redirector-00
                uri += "#_=_";
            }

            return uri;
        }

        const string _formPostHtml = "<form method='post' action='{uri}'>{body}</form><script>(function(){document.forms[0].submit();})();</script>";

        string GetFormPostHtml()
        {
            var html = _formPostHtml;

            html = html.Replace("{uri}", Response.Request.RedirectUri);
            html = html.Replace("{body}", Response.ToNameValueCollection().ToFormPost());

            return html;
        }

        async Task RedirectToErrorPageAsync(HttpContext context)
        {
            var errorModel = new ErrorMessage
            {
                RequestId = context.TraceIdentifier,
                Error = Response.Error
            };

            var message = new MessageWithId<ErrorMessage>(errorModel);
            await _errorMessageStore.WriteAsync(message.Id, message);

            var errorUrl = _options.UserInteractionOptions.ErrorUrl;

            var url = errorUrl.AddQueryString(_options.UserInteractionOptions.ErrorIdParameter, message.Id);
            context.Response.RedirectToAbsoluteUrl(url);
        }
    }
}
