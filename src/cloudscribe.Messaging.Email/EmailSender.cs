//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-07-22
//// Last Modified:			2016-04-21
//// 

//using MailKit.Net.Smtp;
//using MimeKit;
//using System;
//using System.Threading.Tasks;

//namespace cloudscribe.Messaging.Email
//{

//    public class EmailSender
//    {
//        public EmailSender()
//        {
//        }

//        public async Task SendEmailAsync(
//            SmtpOptions smtpOptions,
//            string to,
//            string from,
//            string subject,
//            string plainTextMessage,
//            string htmlMessage,
//            string replyTo = null)
//        {
//            if (string.IsNullOrWhiteSpace(to))
//            {
//                throw new ArgumentException("no to address provided");
//            }

//            if (string.IsNullOrWhiteSpace(from))
//            {
//                throw new ArgumentException("no from address provided");
//            }

//            if (string.IsNullOrWhiteSpace(subject))
//            {
//                throw new ArgumentException("no subject provided");
//            }

//            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
//            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
//            if (!hasPlainText && !hasHtml)
//            {
//                throw new ArgumentException("no message provided");
//            }

//            var m = new MimeMessage();
           
//            m.From.Add(new MailboxAddress("", from));
//            if(!string.IsNullOrWhiteSpace(replyTo))
//            {
//                m.ReplyTo.Add(new MailboxAddress("", replyTo));
//            }
//            m.To.Add(new MailboxAddress("", to));
//            m.Subject = subject;

//            //m.Importance = MessageImportance.Normal;
//            //Header h = new Header(HeaderId.Precedence, "Bulk");
//            //m.Headers.Add()

//            BodyBuilder bodyBuilder = new BodyBuilder();
//            if(hasPlainText)
//            {
//                bodyBuilder.TextBody = plainTextMessage;
//            }

//            if (hasHtml)
//            {
//                bodyBuilder.HtmlBody = htmlMessage;
//            }

//            m.Body = bodyBuilder.ToMessageBody();
            
//            using (var client = new SmtpClient())
//            {
//                //client.ServerCertificateValidationCallback = delegate (
//                //    Object obj, X509Certificate certificate, X509Chain chain,
//                //    SslPolicyErrors errors)
//                //{
//                //    return (true);
//                //};

//                await client.ConnectAsync(
//                    smtpOptions.Server, 
//                    smtpOptions.Port, 
//                    smtpOptions.UseSsl)
//                    .ConfigureAwait(false);
//                //await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls);

//                // Note: since we don't have an OAuth2 token, disable
//                // the XOAUTH2 authentication mechanism.
//                client.AuthenticationMechanisms.Remove("XOAUTH2");

//                // Note: only needed if the SMTP server requires authentication
//                if(smtpOptions.RequiresAuthentication)
//                {
//                    await client.AuthenticateAsync(smtpOptions.User, smtpOptions.Password)
//                        .ConfigureAwait(false);
//                }
                
//                await client.SendAsync(m).ConfigureAwait(false);
//                await client.DisconnectAsync(true).ConfigureAwait(false);
//            }

//        }

//        public async Task SendMultipleEmailAsync(
//            SmtpOptions smtpOptions,
//            string toCsv,
//            string from,
//            string subject,
//            string plainTextMessage,
//            string htmlMessage)
//        {
//            if (string.IsNullOrWhiteSpace(toCsv))
//            {
//                throw new ArgumentException("no to addresses provided");
//            }

//            if (string.IsNullOrWhiteSpace(from))
//            {
//                throw new ArgumentException("no from address provided");
//            }

//            if (string.IsNullOrWhiteSpace(subject))
//            {
//                throw new ArgumentException("no subject provided");
//            }

//            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
//            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
//            if (!hasPlainText && !hasHtml)
//            {
//                throw new ArgumentException("no message provided");
//            }




//            var m = new MimeMessage();

//            m.From.Add(new MailboxAddress("", from));

//            string[] adrs = toCsv.Split(',');

//            foreach (string item in adrs)
//            {
//                if (!string.IsNullOrEmpty(item)) { m.To.Add(new MailboxAddress("", item)); ; }
//            }

//            m.Subject = subject;
//            m.Importance = MessageImportance.High;
           
//            BodyBuilder bodyBuilder = new BodyBuilder();
//            if (hasPlainText)
//            {
//                bodyBuilder.TextBody = plainTextMessage;
//            }

//            if (hasHtml)
//            {
//                bodyBuilder.HtmlBody = htmlMessage;
//            }

//            m.Body = bodyBuilder.ToMessageBody();

//            using (var client = new SmtpClient())
//            {
//                //client.ServerCertificateValidationCallback = delegate (
//                //    Object obj, X509Certificate certificate, X509Chain chain,
//                //    SslPolicyErrors errors)
//                //{
//                //    return (true);
//                //};

//                await client.ConnectAsync(
//                    smtpOptions.Server, 
//                    smtpOptions.Port, 
//                    smtpOptions.UseSsl).ConfigureAwait(false);
//                //await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls);

//                // Note: since we don't have an OAuth2 token, disable
//                // the XOAUTH2 authentication mechanism.
//                client.AuthenticationMechanisms.Remove("XOAUTH2");

//                // Note: only needed if the SMTP server requires authentication
//                if (smtpOptions.RequiresAuthentication)
//                {
//                    await client.AuthenticateAsync(
//                        smtpOptions.User, 
//                        smtpOptions.Password).ConfigureAwait(false);
//                }

//                await client.SendAsync(m).ConfigureAwait(false);
//                await client.DisconnectAsync(true).ConfigureAwait(false);
//            }

//        }

//        //public async Task SendEmailAsync(
//        //    SmtpOptions smtpOptions,
//        //    string toAddress,
//        //    string toName,
//        //    string from,
//        //    string fromName,
//        //    string replyToAddress,
//        //    string cc,
//        //    string bcc,
//        //    string subject,
//        //    string plainTextMessage,
//        //    string htmlMessage)
//        //{


//        //}


//    }
//}
