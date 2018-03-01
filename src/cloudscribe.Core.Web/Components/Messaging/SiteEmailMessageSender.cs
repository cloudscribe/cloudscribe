// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2018-03-01
// 

using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
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
            //ISmtpOptionsProvider smtpOptionsProvider,
            //IOptions<SmtpOptions> smtpOptionsAccessor,
            IStringLocalizer<CloudscribeCore> localizer,
            ILogger<SiteEmailMessageSender> logger
            )
        {
            log = logger;
            sr = localizer;
            this.viewRenderer = viewRenderer;
            _emailSenderResolver = emailSenderResolver;
            //this.smtpOptionsProvider = smtpOptionsProvider;
            //globalSmtpSettings = smtpOptionsAccessor.Value;

        }

        private ViewRenderer viewRenderer;
        private IEmailSenderResolver _emailSenderResolver;
        //private ISmtpOptionsProvider smtpOptionsProvider;
        //private SmtpOptions globalSmtpSettings;
        private IStringLocalizer sr;
        private ILogger log;

        //private async Task<SmtpOptions> GetSmptOptions()
        //{
        //    return await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);
            
        //}

        public async Task SendAccountConfirmationEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string confirmationUrl)
        {
            //var smtpOptions = await GetSmptOptions().ConfigureAwait(false);
            //if (smtpOptions == null)
            //{
            //    var logMessage = $"failed to send account confirmation email because smtp settings are not populated for site {siteSettings.SiteName}";
            //    log.LogError(logMessage);
            //    return;
            //}

            //var sender = new EmailSender();
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send account confirmation email because email settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }


            try
            {
                var plainTextMessage
                = await viewRenderer.RenderViewAsString<string>("EmailTemplates/ConfirmAccountTextEmail", confirmationUrl).ConfigureAwait(false);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/ConfirmAccountHtmlEmail", confirmationUrl).ConfigureAwait(false);

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
                log.LogError("error sending account confirmation email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task SendPasswordResetEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string resetUrl)
        {
            //var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            //if (smtpOptions == null)
            //{
            //    var logMessage = $"failed to send password reset email because smtp settings are not populated for site {siteSettings.SiteName}";
            //    log.LogError(logMessage);
            //    return;
            //}

            //var sender = new EmailSender();
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send password reset email because email settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }
            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<string>("EmailTemplates/PasswordResetTextEmail", resetUrl);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/PasswordResetHtmlEmail", resetUrl);

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
                log.LogError("error sending password reset email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task SendSecurityCodeEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            //var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            //if (smtpOptions == null)
            //{
            //    var logMessage = $"failed to send security code email because smtp settings are not populated for site {siteSettings.SiteName}";
            //    log.LogError(logMessage);
            //    return;
            //}

            //var sender = new EmailSender();
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send security code email because email settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<string>("EmailTemplates/SendSecurityCodeTextEmail", securityCode);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/SendSecurityCodeHtmlEmail", securityCode);

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
                log.LogError("error sending security code email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }

        public async Task AccountPendingApprovalAdminNotification(
            ISiteContext siteSettings,
            IUserContext user)
        {
            if (siteSettings.AccountApprovalEmailCsv.Length == 0) { return; }

            //var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            //if (smtpOptions == null)
            //{
            //    var logMessage = $"failed to send new account notifications to admins because smtp settings are not populated for site {siteSettings.SiteName}";
            //    log.LogError(logMessage);
            //    return;
            //}

            string subject = sr["New Account Pending Approval"];

            //var sender = new EmailSender();
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send new account notifications to admins because email settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<IUserContext>("EmailTemplates/AccountPendingApprovalAdminNotificationTextEmail", user).ConfigureAwait(false);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<IUserContext>("EmailTemplates/AccountPendingApprovalAdminNotificationHtmlEmail", user).ConfigureAwait(false);

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
                log.LogError("error sending new account notification to admins: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }

        public async Task SendAccountApprovalNotificationAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string loginUrl)
        {
            //var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            //if (smtpOptions == null)
            //{
            //    var logMessage = $"failed to send account approval email because smtp settings are not populated for site {siteSettings.SiteName}";
            //    log.LogError(logMessage);
            //    return;
            //}

            //var sender = new EmailSender();
            var sender = await _emailSenderResolver.GetEmailSender(siteSettings.Id.ToString());
            if (sender == null)
            {
                var logMessage = $"failed to send account approval email because email settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<string>("EmailTemplates/AccountApprovedTextEmail", loginUrl);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/AccountApprovedHtmlEmail", loginUrl);

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
                log.LogError("error sending password reset email: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }

        }


    }


}
