// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-02-03
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Messaging.Sms;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteSmsSender : ISmsSender
    {
        public SiteSmsSender(ILogger<SiteSmsSender> logger)
        {
            log = logger;
        }

        private ILogger log;

        public async Task SendSmsAsync(
            ISiteSettings site,
            string phoneNumber, 
            string message)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentException("toPhoneNumber was not provided");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message was not provided");
            }

            var credentials = GetCredentials(site);
            if(credentials == null)
            {
                log.LogError("tried to send sms message with no credentials");
                return;
            }

            TwilioSmsSender sender = new TwilioSmsSender(log);
            try
            {
                await sender.SendMessage(
                    credentials, 
                    phoneNumber, 
                    message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending twilio message", ex);
            }
                
        }

        private TwilioSmsCredentials GetCredentials(ISiteSettings site)
        {
            if(site == null) { return null; }
            if(string.IsNullOrEmpty(site.SmsClientId)) { return null; }
            if (string.IsNullOrEmpty(site.SmsSecureToken)) { return null; }
            if (string.IsNullOrEmpty(site.SmsFrom)) { return null; }

            TwilioSmsCredentials creds = new TwilioSmsCredentials();
            creds.AccountSid = site.SmsClientId;
            creds.AuthToken = site.SmsSecureToken;
            creds.FromNumber = site.SmsFrom;

            return creds;

        }
    }
}
