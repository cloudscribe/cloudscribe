// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2018-05-07
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Email;
using cloudscribe.Email;
using cloudscribe.Web.Common.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailMessageSender : ISiteMessageEmailSender
    {
        //TODO: we should have an option to force only plain text email
        // html emails are a lot more likely to be phished with copies
        // because the link urls are obfuscated to some degree

        public SiteEmailMessageSender(
            ViewRenderer viewRenderer,
            IEmailSenderResolver emailSenderResolver,
            IStringLocalizer<CloudscribeCore> localizer,
            ILogger<SiteEmailMessageSender> logger
            )
        {
            _log = logger;
            _sr = localizer;
            _viewRenderer = viewRenderer;
            _emailSenderResolver = emailSenderResolver;
            
        }

        private ViewRenderer _viewRenderer;
        private IEmailSenderResolver _emailSenderResolver;
        private IStringLocalizer _sr;
        private ILogger _log;
        
        public async Task SendAccountConfirmationEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string confirmationUrl,
            string confirmCode)
        {
            
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send account confirmation email because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new ConfirmEmailAddessViewModel
            {
                ConfirmationUrl = confirmationUrl,
                ConfirmationCode = confirmCode,
                Tenant = siteSettings
            };


            try
            {
                var plainTextMessage
                = await _viewRenderer.RenderViewAsString<ConfirmEmailAddessViewModel>("EmailTemplates/ConfirmAccountTextEmail", model).ConfigureAwait(false);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<ConfirmEmailAddessViewModel>("EmailTemplates/ConfirmAccountHtmlEmail", model).ConfigureAwait(false);

                await sender.SendEmailAsync(
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()
                    
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError($"error sending account confirmation email: {ex.Message} stacktrace: {ex.StackTrace}");
            }

        }

        public async Task SendPasswordResetEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string resetUrl)
        {
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send password reset email because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new PasswordResetEmailViewModel
            {
                Tenant = siteSettings,
                ResetUrl = resetUrl
            };
            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<PasswordResetEmailViewModel>("EmailTemplates/PasswordResetTextEmail", model);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<PasswordResetEmailViewModel>("EmailTemplates/PasswordResetHtmlEmail", model);

                await sender.SendEmailAsync(
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError("error sending password reset email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task SendSecurityCodeEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send security code email because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new SecurityCodeEmailViewModel
            {
                Tenant = siteSettings,
                SecurityCode = securityCode
            };

            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<SecurityCodeEmailViewModel>("EmailTemplates/SendSecurityCodeTextEmail", model);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<SecurityCodeEmailViewModel>("EmailTemplates/SendSecurityCodeHtmlEmail", model);

                await sender.SendEmailAsync(
                toAddress,
                siteSettings.DefaultEmailFromAddress,
                subject,
                plainTextMessage,
                htmlMessage,
                configLookupKey: siteSettings.Id.ToString()
                ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError("error sending security code email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }

        public async Task AccountPendingApprovalAdminNotification(
            ISiteContext siteSettings,
            IUserContext user)
        {
            
            if (siteSettings.AccountApprovalEmailCsv == null || siteSettings.AccountApprovalEmailCsv.Trim().Length == 0) 
            { 
                var logMessage = $"failed to send new account approval notifications to admins because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return; 
            }
            
            string subject = _sr["New Account Pending Approval"];

            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send new account approval notifications to admins because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new AccountPendingApprovalEmailViewModel
            {
                Tenant = siteSettings,
                User = user
            };

            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<AccountPendingApprovalEmailViewModel>("EmailTemplates/AccountPendingApprovalAdminNotificationTextEmail", model).ConfigureAwait(false);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<AccountPendingApprovalEmailViewModel>("EmailTemplates/AccountPendingApprovalAdminNotificationHtmlEmail", model).ConfigureAwait(false);

                await sender.SendEmailAsync(
                    siteSettings.AccountApprovalEmailCsv,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError("error sending new account notification to admins: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task NewAccountAdminNotification(
            ISiteContext siteSettings,
            IUserContext user)
        {
            if (siteSettings.AccountApprovalEmailCsv == null || siteSettings.AccountApprovalEmailCsv.Trim().Length == 0) { return; }
            
            string subject = _sr["New Account"];

            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send new account notifications to admins because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new NewAccountEmailViewModel
            {
                Tenant = siteSettings,
                User = user
            };

            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<NewAccountEmailViewModel>("EmailTemplates/NewAccountAdminNotificationTextEmail", model).ConfigureAwait(false);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<NewAccountEmailViewModel>("EmailTemplates/NewAccountAdminNotificationHtmlEmail", model).ConfigureAwait(false);

                await sender.SendEmailAsync(
                    siteSettings.AccountApprovalEmailCsv,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError("error sending new account notification to admins: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task SendAccountApprovalNotificationAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string loginUrl)
        {
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send account approval email because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            var model = new AccountApprovedEmailViewModel
            {
                Tenant = siteSettings,
                LoginUrl = loginUrl
            };

            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                var plainTextMessage
                   = await _viewRenderer.RenderViewAsString<AccountApprovedEmailViewModel>("EmailTemplates/AccountApprovedTextEmail", model);

                var htmlMessage
                    = await _viewRenderer.RenderViewAsString<AccountApprovedEmailViewModel>("EmailTemplates/AccountApprovedHtmlEmail", model);

                await sender.SendEmailAsync(
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError("error sending password reset email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }


        public async Task SendSiteMessage(
            ISiteContext siteSettings,
            SiteMessageModel model,
            string baseUrl
            )
        {
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send account confirmation email because email settings are not populated for site {siteSettings.SiteName}";
                _log.LogError(logMessage);
                return;
            }

            string plainTextMessage = null;
            string htmlMessage = null;

            if (model.Tenant == null) model.Tenant = siteSettings;
            if (string.IsNullOrWhiteSpace(model.ConfigLookupKey)) model.ConfigLookupKey = sender.Name;
            
            try
            {
                if(!string.IsNullOrWhiteSpace(model.TextBody))
                {
                    plainTextMessage = await _viewRenderer.RenderViewAsString<SiteMessageModel>("SiteMessageTextPartial", model).ConfigureAwait(false);
                }
                
                if (!string.IsNullOrWhiteSpace(model.HtmlBody))
                {
                    htmlMessage = await _viewRenderer.RenderViewAsString<SiteMessageModel>("SiteMessageHtmlPartial", model).ConfigureAwait(false);
                    if(!string.IsNullOrWhiteSpace(baseUrl))
                    {
                        htmlMessage = cloudscribe.Web.Common.Html.HtmlHelper.ConvertUrlsToAbsolute(baseUrl, htmlMessage);
                    } 
                }
                
                await sender.SendEmailAsync(
                    model.ToEmailCsv,
                    siteSettings.DefaultEmailFromAddress,
                    model.Subject.Replace("{SenderName}", model.ConfigLookupKey),
                    plainTextMessage,
                    htmlMessage,
                    configLookupKey: siteSettings.Id.ToString()

                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError($"error sending account confirmation email: {ex.Message} stacktrace: {ex.StackTrace}");
            }

        }


    }


}
