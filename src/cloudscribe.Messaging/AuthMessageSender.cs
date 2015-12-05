// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2015-12-05
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace cloudscribe.Messaging
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var m = new MimeMessage();
            // TODO: need tenant specific from address and smtp settings
            m.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
            m.To.Add(new MailboxAddress("", email));
            m.Subject = subject;
            m.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.friends.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("joey", "password");

                client.Send(m);
                client.Disconnect(true);
            }

            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }


}
