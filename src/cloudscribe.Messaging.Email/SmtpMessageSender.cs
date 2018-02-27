// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-27
// Last Modified:			2018-02-27
// 

using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{
    public class SmtpMessageSender : IMessageSender
    {
        public SmtpMessageSender(ISmtpOptionsProvider smtpOptionsProvider)
        {
            _smtpOptionsProvider = smtpOptionsProvider;
        }

        private ISmtpOptionsProvider _smtpOptionsProvider;

        private async Task<SmtpOptions> GetSmptOptions()
        {
            return await _smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);
            
        }

        private MessageImportance GetMessageImportance(Importance importance)
        {
            switch (importance)
            {
                case Importance.Low:
                    return MessageImportance.Low;
                case Importance.High:
                    return MessageImportance.High;
                case Importance.Normal:
                default:
                    return MessageImportance.Normal;
            }
        }

        public string Name { get; } = "SmtpMailSender";

        public async Task SendEmailAsync(
            string toEmailCsv,
            string fromEmail,
            string subject,
            string plainTextMessage,
            string htmlMessage,
            string replyToEmail = null,
            Importance importance = Importance.Normal,
            bool isTransactional = true,
            string fromName = null,
            string replyToName = null,
            string toAliasCsv = null,
            string ccEmailCsv = null,
            string ccAliasCsv = null,
            string bccEmailCsv = null,
            string bccAliasCsv = null,
            string[] attachmentFilePaths = null
            )
        {
            if (string.IsNullOrWhiteSpace(toEmailCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
            if (!hasPlainText && !hasHtml)
            {
                throw new ArgumentException("no message provided");
            }

            var m = new MimeMessage();

            m.From.Add(new MailboxAddress(fromName, fromEmail));
            if (!string.IsNullOrWhiteSpace(replyToEmail))
            {
                m.ReplyTo.Add(new MailboxAddress(replyToName, replyToEmail));
            }

            if (toEmailCsv.Contains(","))
            {
                var useToAliases = false;
                string[] adrs = toEmailCsv.Split(',');
                string[] toAliases = new string[0];
                if (toAliasCsv != null && toAliasCsv.Contains(","))
                {
                    toAliases = toAliasCsv.Split(',');
                    if(toAliases.Length > 0 && toAliases.Length == adrs.Length)
                    {
                        useToAliases = true;
                    }
                }
                if(useToAliases)
                {
                    for(int i = 0; i< adrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(adrs[i])) { m.To.Add(new MailboxAddress(toAliases[i].Trim(), adrs[i].Trim())); }
                    }
                }
                else
                {
                    foreach (string item in adrs)
                    {
                        if (!string.IsNullOrEmpty(item)) { m.To.Add(new MailboxAddress("", item.Trim())); }
                    }
                }
                
            }
            else
            {
                m.To.Add(new MailboxAddress(toAliasCsv, toEmailCsv));
            }

            if(!string.IsNullOrWhiteSpace(ccEmailCsv))
            {
                if (ccEmailCsv.Contains(","))
                {
                    var useAliases = false;
                    string[] adrs = ccEmailCsv.Split(',');
                    string[] aliases = new string[0];
                    if (ccAliasCsv != null && ccAliasCsv.Contains(","))
                    {
                        aliases = ccAliasCsv.Split(',');
                        if (aliases.Length > 0 && aliases.Length == adrs.Length)
                        {
                            useAliases = true;
                        }
                    }
                    if (useAliases)
                    {
                        for (int i = 0; i < adrs.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(adrs[i])) { m.Cc.Add(new MailboxAddress(aliases[i].Trim(), adrs[i].Trim())); }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item)) { m.Cc.Add(new MailboxAddress("", item.Trim())); }
                        }
                    }

                }
                else
                {
                    m.Cc.Add(new MailboxAddress(ccAliasCsv, ccEmailCsv));
                }
            }

            if (!string.IsNullOrWhiteSpace(bccEmailCsv))
            {
                if (bccEmailCsv.Contains(","))
                {
                    var useAliases = false;
                    string[] adrs = bccEmailCsv.Split(',');
                    string[] aliases = new string[0];
                    if (bccAliasCsv != null && bccAliasCsv.Contains(","))
                    {
                        aliases = bccAliasCsv.Split(',');
                        if (aliases.Length > 0 && aliases.Length == adrs.Length)
                        {
                            useAliases = true;
                        }
                    }
                    if (useAliases)
                    {
                        for (int i = 0; i < adrs.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(adrs[i])) { m.Bcc.Add(new MailboxAddress(aliases[i].Trim(), adrs[i].Trim())); }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item)) { m.Bcc.Add(new MailboxAddress("", item.Trim())); }
                        }
                    }

                }
                else
                {
                    m.Bcc.Add(new MailboxAddress(bccAliasCsv, bccEmailCsv));
                }
            }


            m.Subject = subject;
            m.Importance = GetMessageImportance(importance);

            if(!isTransactional)
            {
                var h = new Header(HeaderId.Precedence, "Bulk");
                m.Headers.Add(h);
            }

            var bodyBuilder = new BodyBuilder();
            if (hasPlainText)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (hasHtml)
            {
                bodyBuilder.HtmlBody = htmlMessage;
            }

            if(attachmentFilePaths != null && attachmentFilePaths.Length > 0)
            {
                foreach(var filePath in attachmentFilePaths)
                {
                    bodyBuilder.Attachments.Add(filePath);
                }
            }

            m.Body = bodyBuilder.ToMessageBody();
            
            

            var smtpOptions = await GetSmptOptions();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    smtpOptions.Server,
                    smtpOptions.Port,
                    smtpOptions.UseSsl).ConfigureAwait(false);

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


        
    }
}
