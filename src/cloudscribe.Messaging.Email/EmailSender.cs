// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-01-19
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace cloudscribe.Messaging.Email
{
    
    public class EmailSender
    {
        public EmailSender()
        {
        }

        public async Task SendEmailAsync(
            SmtpOptions smtpOptions,
            string to,
            string from,
            string subject,
            string plainTextMessage,
            string htmlMessage)
        {
            if(string.IsNullOrEmpty(plainTextMessage) && string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentException("no message provided");
            }

            var m = new MimeMessage();
           
            m.From.Add(new MailboxAddress("", from));
            m.To.Add(new MailboxAddress("", to));
            m.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            if(plainTextMessage.Length > 0)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (htmlMessage.Length > 0)
            {
                bodyBuilder.HtmlBody = htmlMessage;
            }

            

            m.Body = bodyBuilder.ToMessageBody();

            //var textPart = new TextPart("plain") { Text = plainTextMessage };
            //var htmlPart = new HtmlPart("plain") { Text = plainTextMessage };

            //m.Body = new TextPart("plain") { Text = plainTextMessage };

            using (var client = new SmtpClient())
            {
                //client.ServerCertificateValidationCallback = delegate (
                //    Object obj, X509Certificate certificate, X509Chain chain,
                //    SslPolicyErrors errors)
                //{
                //    return (true);
                //};

                await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, smtpOptions.UseSsl);
                //await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if(smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(smtpOptions.User, smtpOptions.Password);
                }
                
                client.Send(m);
                client.Disconnect(true);
            }

        }
    }
}
