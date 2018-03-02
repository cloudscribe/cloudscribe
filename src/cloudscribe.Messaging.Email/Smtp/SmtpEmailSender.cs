// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-27
// Last Modified:			2018-03-01
// 

using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

//TODO: not sure but we may need some changes in this class to support some languages
// open to a pull request
//https://stackoverflow.com/questions/7266935/how-to-send-utf-8-email
//https://stackoverflow.com/questions/15566632/different-content-types-in-email
//http://www.mimekit.net/docs/html/T_MimeKit_TextPart.htm
//http://www.mimekit.net/docs/html/M_MimeKit_TextPart_SetText_1.htm

namespace cloudscribe.Messaging.Email.Smtp
{
    public class SmtpEmailSender : IEmailSender
    {
        public SmtpEmailSender(
            ISmtpOptionsProvider smtpOptionsProvider,
            ILogger<SmtpEmailSender> logger
            )
        {
            _smtpOptionsProvider = smtpOptionsProvider;
            _log = logger;
        }

        private ISmtpOptionsProvider _smtpOptionsProvider;
        private ILogger _log;
        
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

        private SmtpOptions smtpOptions = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(smtpOptions == null)
            {
                smtpOptions = await _smtpOptionsProvider.GetSmtpOptions(configLookupKey).ConfigureAwait(false);
            }
            
            if (smtpOptions == null) return false;

            return !string.IsNullOrWhiteSpace(smtpOptions.Server);
        }

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
            string[] attachmentFilePaths = null,
            string charsetBodyHtml = null, // not currently used in this implementation
            string charsetBodyText = null, //not currently used in this implementation
            string configLookupKey = null
            )
        {
            var isConfigured = await IsConfigured(configLookupKey);
          
            if (!isConfigured)
            {
                _log.LogError($"failed to send email with subject {subject} because smtp options are not configured");

                return;
            }

            if (string.IsNullOrWhiteSpace(toEmailCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(fromEmail) && string.IsNullOrWhiteSpace(smtpOptions.DefaultEmailFromAddress))
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
            if(!string.IsNullOrEmpty(fromEmail))
            {
                m.From.Add(new MailboxAddress(fromName, fromEmail));
            }
            else
            {
                m.From.Add(new MailboxAddress(smtpOptions.DefaultEmailFromAlias, smtpOptions.DefaultEmailFromAddress));
            }
            

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
                var h = new Header(HeaderId.Precedence, "bulk");
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
                    try
                    {
                        var bytes = File.ReadAllBytes(filePath);
                        bodyBuilder.Attachments.Add(Path.GetFileName(filePath), bytes);
                    }
                    catch(Exception ex)
                    {
                        _log.LogError($"failed to add attachment with path {filePath}, error was {ex.Message} : {ex.StackTrace}");
                    }

                    
                }
            }
            
            m.Body = bodyBuilder.ToMessageBody();
            
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

                try
                {
                    await client.SendAsync(m).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
                }
                
            }

        }


        
    }
}
