// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2016-02-03
// 

using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.Logging;


namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailMessageSender : ISiteMessageEmailSender
    {
        //TODO: we should have an option to force only plain text email
        // html emails are a lot more likely to be phished with copies
        // because the link urls are obfuscated to some degree

        public SiteEmailMessageSender(
            ILogger<SiteEmailMessageSender> logger,
            IEmailTemplateService templateService = null)
        {
            log = logger;

            if(templateService == null)
            {
                this.templateService = new HardCodedEmailTemplateService();
            }
            else
            {
                this.templateService = templateService;
            }
            
        }

        private IEmailTemplateService templateService;
        private ILogger log;

        private SmtpOptions GetSmptOptions(ISiteSettings siteSettings)
        {
            if(string.IsNullOrEmpty(siteSettings.SmtpServer)) { return null; }

            SmtpOptions smtpOptions = new SmtpOptions();
            smtpOptions.Password = siteSettings.SmtpPassword;
            smtpOptions.Port = siteSettings.SmtpPort;
            smtpOptions.PreferredEncoding = siteSettings.SmtpPreferredEncoding;
            smtpOptions.RequiresAuthentication = siteSettings.SmtpRequiresAuth;
            smtpOptions.Server = siteSettings.SmtpServer;
            smtpOptions.User = siteSettings.SmtpUser;
            smtpOptions.UseSsl = siteSettings.SmtpUseSsl;

            return smtpOptions;
        }

        public async Task SendAccountConfirmationEmailAsync(
            ISiteSettings siteSettings,
            string toAddress, 
            string subject, 
            string confirmationUrl)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);
            if(smtpOptions == null)
            {
                var logMessage = $"failed to send account confirmation email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage = string.Format(plainTextTemplate, confirmationUrl);
            
            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            string htmlMessage = string.Format(htmlTemplate, confirmationUrl);
            
            EmailSender sender = new EmailSender();
            try
            {
                await sender.SendEmailAsync(
                    smtpOptions,
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
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
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string resetUrl)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send password reset email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.PasswordReset, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage 
                = string.Format(
                    CultureInfo.InvariantCulture, 
                    plainTextTemplate,
                    siteSettings.SiteName, //maybe this should be the site url?
                    resetUrl);

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.PasswordReset, CultureInfo.CurrentUICulture.Name);
            string htmlMessage 
                = string.Format(
                    CultureInfo.InvariantCulture,
                    htmlTemplate,
                    siteSettings.SiteName, 
                    resetUrl);

            EmailSender sender = new EmailSender();
            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                await sender.SendEmailAsync(
                    smtpOptions,
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    plainTextMessage,
                    htmlMessage).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                log.LogError("error sending password reset email", ex);
            }
            
        }

        public async Task SendSecurityCodeEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send security code email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.SendSecurityCode, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage = string.Format(plainTextTemplate, securityCode);

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.SendSecurityCode, CultureInfo.CurrentUICulture.Name);
            string htmlMessage = string.Format(htmlTemplate, securityCode);


            EmailSender sender = new EmailSender();
            try
            {
                await sender.SendEmailAsync(
                smtpOptions,
                toAddress,
                siteSettings.DefaultEmailFromAddress,
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
            ISiteSettings siteSettings,
            ISiteUser user)
        {
            if(siteSettings.AccountApprovalEmailCsv.Length == 0) { return; }

            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send new account notifications to admins because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string subject = "New Account Pending Approval";
            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            //string plainTextMessage = string.Format(plainTextTemplate, confirmationUrl);
            //string plainTextMessage = "U"
            var message = $"A new user just registered at {siteSettings.SiteName} with email address {user.Email}";

            //string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            //string htmlMessage = string.Format(htmlTemplate, confirmationUrl);

            EmailSender sender = new EmailSender();
            try
            {
                await sender.SendMultipleEmailAsync(
                    smtpOptions,
                    siteSettings.AccountApprovalEmailCsv,
                    siteSettings.DefaultEmailFromAddress,
                    subject,
                    message,
                    string.Empty).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending email verification email", ex);
            }
            
        }

        public async Task SendAccountApprovalNotificationAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string loginUrl)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send account approval email because smtp settings are not populated for site {siteSettings.SiteName}";
                log.LogError(logMessage);
                return;
            }

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.AccountApproved, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage 
                = string.Format(
                    CultureInfo.InvariantCulture,
                    plainTextTemplate, 
                    siteSettings.SiteName,
                    loginUrl
                    );

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.AccountApproved, CultureInfo.CurrentUICulture.Name);
            string htmlMessage 
                = string.Format(
                    CultureInfo.InvariantCulture,
                    htmlTemplate,
                    siteSettings.SiteName,
                    loginUrl);

            EmailSender sender = new EmailSender();
            // in account controller we are calling this method without await
            // so it doesn't block the UI. Which means it is running on a background thread
            // similar as the old ThreadPool.QueueWorkItem
            // as such we need to handle any error that may happen so it doesn't
            // brind down the thread or the process
            try
            {
                await sender.SendEmailAsync(
                    smtpOptions,
                    toAddress,
                    siteSettings.DefaultEmailFromAddress,
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
