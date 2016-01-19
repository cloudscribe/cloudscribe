// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2016-01-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteEmailMessageSender : ISiteMessageEmailSender
    {
        public SiteEmailMessageSender(IEmailTemplateService templateService = null)
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

            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.ConfirmAccount, "");
            string plainTextMessage = string.Format(plainTextTemplate, confirmationUrl);
            
            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.ConfirmAccount, "");
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

        public async Task SendSecurityCodeEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string securityCode)
        {
            SmtpOptions smtpOptions = GetSmptOptions(siteSettings);
            
            string plainTextTemplate = templateService.GetPlainTextTemplate(MessagePurpose.SendSecurityCode, "");
            string plainTextMessage = string.Format(plainTextTemplate, securityCode);

            string htmlTemplate = templateService.GetHtmlTemplate(MessagePurpose.SendSecurityCode, "");
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

        
    }


}
