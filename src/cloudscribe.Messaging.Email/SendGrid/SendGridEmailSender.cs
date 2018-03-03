// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-28
// Last Modified:			2018-03-03
// 

using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(
            ISendGridOptionsProvider optionsProvider,
            ILogger<SendGridEmailSender> logger
            )
        {
            _optionsProvider = optionsProvider;
            _log = logger;
        }

        private ISendGridOptionsProvider _optionsProvider;
        private ILogger _log;

        private string GetMessageImportance(Importance importance)
        {
            switch (importance)
            {
                case Importance.Low:
                    return "low";
                case Importance.High:
                    return "high";
                case Importance.Normal:
                default:
                    return "normal";
            }
        }


        public string Name { get; } = "SendGridEmailSender";

        private SendGridOptions options = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(options == null)
            {
                options = await _optionsProvider.GetSendGridOptions(configLookupKey);
            }
            
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey))
            {
                return false;
            }
            return true;

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
            string charsetBodyHtml = null,
            string charsetBodyText = null,
            string configLookupKey = null
            )
        {
            var isConfigured = await IsConfigured(configLookupKey);
            
            if(!isConfigured)
            {
                _log.LogError($"failed to send email with subject {subject} because sendgrid api key is empty or not configured");
                return;
            }

            if (string.IsNullOrWhiteSpace(toEmailCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(fromEmail) && string.IsNullOrWhiteSpace(options.DefaultEmailFromAddress))
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

            var m = new SendGridMessage();
            if(!string.IsNullOrWhiteSpace(fromEmail))
            {
                m.From = new EmailAddress(fromEmail, fromName);
            }
            else
            {
                m.From = new EmailAddress(options.DefaultEmailFromAddress, options.DefaultEmailFromAlias);
            }
            
            if (!string.IsNullOrWhiteSpace(replyToEmail))
            {
                m.ReplyTo = new EmailAddress(replyToEmail, replyToName);
            }

            m.Subject = subject;
            m.HtmlContent = htmlMessage;
            m.PlainTextContent = plainTextMessage;
           
            if (toEmailCsv.Contains(","))
            {
                var useToAliases = false;
                string[] adrs = toEmailCsv.Split(',');
                string[] toAliases = new string[0];
                if (toAliasCsv != null && toAliasCsv.Contains(","))
                {
                    toAliases = toAliasCsv.Split(',');
                    if (toAliases.Length > 0 && toAliases.Length == adrs.Length)
                    {
                        useToAliases = true;
                    }
                }
                if (useToAliases)
                {
                    for (int i = 0; i < adrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(adrs[i])) { m.AddTo(new EmailAddress(adrs[i].Trim(), toAliases[i].Trim())); }
                    }
                }
                else
                {
                    foreach (string item in adrs)
                    {
                        if (!string.IsNullOrEmpty(item)) { m.AddTo(new EmailAddress(item.Trim(), "")); }
                    }
                }

            }
            else
            {
                //not really a csv
                m.AddTo(new EmailAddress(toEmailCsv, toAliasCsv));
            }

            if (!string.IsNullOrWhiteSpace(ccEmailCsv))
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
                            if (!string.IsNullOrEmpty(adrs[i])) { m.AddCc(new EmailAddress(adrs[i].Trim(), aliases[i].Trim())); }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item)) { m.AddCc(new EmailAddress(item.Trim(), "")); }
                        }
                    }

                }
                else
                {
                    m.AddCc(new EmailAddress(ccEmailCsv, ccAliasCsv));
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
                            if (!string.IsNullOrEmpty(adrs[i])) { m.AddBcc(new EmailAddress(adrs[i].Trim(), aliases[i].Trim())); }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item)) { m.AddBcc(new EmailAddress(item.Trim(), "")); }
                        }
                    }

                }
                else
                {
                    m.AddBcc(new EmailAddress(bccEmailCsv, bccAliasCsv));
                }
            }

            m.AddHeader("Importance", GetMessageImportance(importance));
            
            if (!isTransactional)
            {
                m.AddHeader("Precedence", "bulk");
            }

            if (attachmentFilePaths != null && attachmentFilePaths.Length > 0)
            {
                foreach (var filePath in attachmentFilePaths)
                {
                    try
                    {
                        var bytes = File.ReadAllBytes(filePath);
                        var content = Convert.ToBase64String(bytes);
                        m.AddAttachment(Path.GetFileName(filePath),content);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"failed to add attachment with path {filePath}, error was {ex.Message} : {ex.StackTrace}");
                    }
                }
            }

            var client = new SendGridClient(options.ApiKey);
            try
            {
                var response = await client.SendEmailAsync(m);
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted && response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _log.LogError($"did not get expected 200 status code from SendGrid, response was {response.StatusCode} {response.Body.ToString()} ");
                }
            }
            catch(Exception ex)
            {
                _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
            }

            

        }
    }
}
