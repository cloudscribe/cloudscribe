// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2017-05-23
// 

using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using cloudscribe.Web.Common.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            ISmtpOptionsProvider smtpOptionsProvider,
            //IOptions<SmtpOptions> smtpOptionsAccessor,
            IStringLocalizer<CloudscribeCore> localizer,
            ILogger<SiteEmailMessageSender> logger
            )
        {
            log = logger;
            sr = localizer;
            this.viewRenderer = viewRenderer;
            this.smtpOptionsProvider = smtpOptionsProvider;
            //globalSmtpSettings = smtpOptionsAccessor.Value;

        }

        private ViewRenderer viewRenderer;
        private ISmtpOptionsProvider smtpOptionsProvider;
        //private SmtpOptions globalSmtpSettings;
        private IStringLocalizer sr;
        private ILogger log;

        private async Task<SmtpOptions> GetSmptOptions()
        {
            return await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);
            //if(!siteSettings.SmtpIsConfigured()) { return globalSmtpSettings; }

            //SmtpOptions smtpOptions = new SmtpOptions();
            //smtpOptions.Password = siteSettings.SmtpPassword;
            //smtpOptions.Port = siteSettings.SmtpPort;
            //smtpOptions.PreferredEncoding = siteSettings.SmtpPreferredEncoding;
            //smtpOptions.RequiresAuthentication = siteSettings.SmtpRequiresAuth;
            //smtpOptions.Server = siteSettings.SmtpServer;
            //smtpOptions.User = siteSettings.SmtpUser;
            //smtpOptions.UseSsl = siteSettings.SmtpUseSsl;
            //smtpOptions.DefaultEmailFromAddress = siteSettings.DefaultEmailFromAddress;
            //smtpOptions.DefaultEmailFromAlias = siteSettings.DefaultEmailFromAlias;

            //return smtpOptions;
        }

        public async Task SendAccountConfirmationEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string confirmationUrl)
        {
            var smtpOptions = await GetSmptOptions().ConfigureAwait(false);
            if (smtpOptions == null)
            {
                var logMessage = $"failed to send account confirmation email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }
            
            var sender = new EmailSender();
            try
            {
                var plainTextMessage
                = await viewRenderer.RenderViewAsString<string>("EmailTemplates/ConfirmAccountTextEmail", confirmationUrl).ConfigureAwait(false);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/ConfirmAccountHtmlEmail", confirmationUrl).ConfigureAwait(false);

                await sender.SendEmailAsync(
                    smtpOptions,
                    toAddress,
                    smtpOptions.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending account confirmation email", ex);
            }

        }

        public async Task SendPasswordResetEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string resetUrl)
        {
            var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send password reset email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }
            
            var sender = new EmailSender();
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
                    smtpOptions,
                    toAddress,
                    smtpOptions.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending password reset email", ex);
            }

        }

        public async Task SendSecurityCodeEmailAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send security code email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }
            
            var sender = new EmailSender();
            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<string>("EmailTemplates/SendSecurityCodeTextEmail", securityCode);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<string>("EmailTemplates/SendSecurityCodeHtmlEmail", securityCode);

                await sender.SendEmailAsync(
                smtpOptions,
                toAddress,
                smtpOptions.DefaultEmailFromAddress,
                subject,
                plainTextMessage,
                htmlMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending security code email", ex);
            }
        }

        public async Task AccountPendingApprovalAdminNotification(
            ISiteContext siteSettings,
            IUserContext user)
        {
            if (siteSettings.AccountApprovalEmailCsv.Length == 0) { return; }

            var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send new account notifications to admins because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string subject = sr["New Account Pending Approval"];
           
            var sender = new EmailSender();
            try
            {
                var plainTextMessage
                   = await viewRenderer.RenderViewAsString<IUserContext>("EmailTemplates/AccountPendingApprovalAdminNotificationTextEmail", user).ConfigureAwait(false);

                var htmlMessage
                    = await viewRenderer.RenderViewAsString<IUserContext>("EmailTemplates/AccountPendingApprovalAdminNotificationHtmlEmail", user).ConfigureAwait(false);

                await sender.SendMultipleEmailAsync(
                    smtpOptions,
                    siteSettings.AccountApprovalEmailCsv,
                    smtpOptions.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending email verification email", ex);
            }

        }

        public async Task SendAccountApprovalNotificationAsync(
            ISiteContext siteSettings,
            string toAddress,
            string subject,
            string loginUrl)
        {
            var smtpOptions = await GetSmptOptions().ConfigureAwait(false);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send account approval email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }
            
            var sender = new EmailSender();
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
                    smtpOptions,
                    toAddress,
                    smtpOptions.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending password reset email", ex);
            }

        }


    }


}
