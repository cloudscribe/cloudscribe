// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2016-02-01
// 

using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;


namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailMessageSender : ISiteMessageEmailSender
    {
        public SiteEmailMessageSender(
            IEmailTemplateService templateService = null)
        {
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

        private SmtpOptions GetSmptOptions(ISiteSettings siteSettings)
        {
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

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage = string.Format(plainTextTemplate, confirmationUrl);
            
            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            string htmlMessage = string.Format(htmlTemplate, confirmationUrl);
            
            EmailSender sender = new EmailSender();
            await sender.SendEmailAsync(
                smtpOptions,
                toAddress,
                siteSettings.DefaultEmailFromAddress,
                subject,
                plainTextMessage,
                htmlMessage);
        }

        public async Task SendPasswordResetEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string resetUrl)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.PasswordReset, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage = string.Format(plainTextTemplate, resetUrl);

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.PasswordReset, CultureInfo.CurrentUICulture.Name);
            string htmlMessage = string.Format(htmlTemplate, resetUrl);

            EmailSender sender = new EmailSender();
            await sender.SendEmailAsync(
                smtpOptions,
                toAddress,
                siteSettings.DefaultEmailFromAddress,
                subject,
                plainTextMessage,
                htmlMessage);
        }

        public async Task SendSecurityCodeEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);
            
            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.SendSecurityCode, CultureInfo.CurrentUICulture.Name);
            string plainTextMessage = string.Format(plainTextTemplate, securityCode);

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.SendSecurityCode, CultureInfo.CurrentUICulture.Name);
            string htmlMessage = string.Format(htmlTemplate, securityCode);


            EmailSender sender = new EmailSender();
            await sender.SendEmailAsync(
                smtpOptions,
                toAddress,
                siteSettings.DefaultEmailFromAddress,
                subject,
                plainTextMessage,
                htmlMessage);

        }

        public async Task AccountPendingApprovalAdminNotification(
            ISiteSettings siteSettings,
            ISiteUser user)
        {
            if(siteSettings.AccountApprovalEmailCsv.Length == 0) { return; }

            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);

            string subject = "New Account Pending Approval";
            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            //string plainTextMessage = string.Format(plainTextTemplate, confirmationUrl);
            //string plainTextMessage = "U"
            var message = $"A new user just registered at {siteSettings.SiteName} with email address {user.Email}";

            //string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.ConfirmAccount, CultureInfo.CurrentUICulture.Name);
            //string htmlMessage = string.Format(htmlTemplate, confirmationUrl);

            EmailSender sender = new EmailSender();
            await sender.SendMultipleEmailAsync(
                smtpOptions,
                siteSettings.AccountApprovalEmailCsv,
                siteSettings.DefaultEmailFromAddress,
                subject,
                message,
                string.Empty);
        }


    }


}
