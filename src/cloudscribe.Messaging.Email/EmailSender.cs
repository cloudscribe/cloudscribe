// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-02-03
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
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("no to address provided");
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            if (string.IsNullOrEmpty(plainTextMessage) && string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentException("no message provided");
            }

            var m = new MimeMessage();
           
            m.From.Add(new MailboxAddress("", from));
            m.To.Add(new MailboxAddress("", to));
            m.Subject = subject;
            //m.Importance = MessageImportance.Normal;
            //Header h = new Header(HeaderId.Precedence, "Bulk");
            //m.Headers.Add()

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
            
            using (var client = new SmtpClient())
            {
                //client.ServerCertificateValidationCallback = delegate (
                //    Object obj, X509Certificate certificate, X509Chain chain,
                //    SslPolicyErrors errors)
                //{
                //    return (true);
                //};

                await client.ConnectAsync(
                    smtpOptions.Server, 
                    smtpOptions.Port, 
                    smtpOptions.UseSsl)
                    .ConfigureAwait(false);
                //await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if(smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(smtpOptions.User, smtpOptions.Password)
                        .ConfigureAwait(false);
                }
                
                await client.SendAsync(m).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }

        public async Task SendMultipleEmailAsync(
            SmtpOptions smtpOptions,
            string toCsv,
            string from,
            string subject,
            string plainTextMessage,
            string htmlMessage)
        {
            if (string.IsNullOrEmpty(toCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            if (string.IsNullOrEmpty(plainTextMessage) && string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentException("no message provided");
            }

            


            var m = new MimeMessage();

            m.From.Add(new MailboxAddress("", from));

            string[] adrs = toCsv.Split(',');

            foreach (string item in adrs)
            {
                if (!string.IsNullOrEmpty(item)) { m.To.Add(new MailboxAddress("", item)); ; }
            }

            m.Subject = subject;
            m.Importance = MessageImportance.High;
           
            BodyBuilder bodyBuilder = new BodyBuilder();
            if (plainTextMessage.Length > 0)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (htmlMessage.Length > 0)
            {
                bodyBuilder.HtmlBody = htmlMessage;
            }

            m.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                //client.ServerCertificateValidationCallback = delegate (
                //    Object obj, X509Certificate certificate, X509Chain chain,
                //    SslPolicyErrors errors)
                //{
                //    return (true);
                //};

                await client.ConnectAsync(
                    smtpOptions.Server, 
                    smtpOptions.Port, 
                    smtpOptions.UseSsl).ConfigureAwait(false);
                //await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if (smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(
                        smtpOptions.User, 
                        smtpOptions.Password).ConfigureAwait(false);
                }

                await client.SendAsync(m).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }

        //public async Task SendEmailAsync(
        //    SmtpOptions smtpOptions,
        //    string toAddress,
        //    string toName,
        //    string from,
        //    string fromName,
        //    string replyToAddress,
        //    string cc,
        //    string bcc,
        //    string subject,
        //    string plainTextMessage,
        //    string htmlMessage)
        //{


        //}


    }
}
