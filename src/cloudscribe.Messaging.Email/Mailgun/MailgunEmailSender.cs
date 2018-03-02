// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-28
// Last Modified:			2018-03-02
// 


using FluentEmail.Mailgun;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

//https://elanderson.net/2017/02/email-with-asp-net-core-using-mailgun/
//https://github.com/lukencode/FluentEmail/blob/master/src/Senders/FluentEmail.Mailgun/MailgunSender.cs
//TODO: not sure but we may need some changes in this class to support some languages
// open to a pull request
//https://stackoverflow.com/questions/7266935/how-to-send-utf-8-email
//https://stackoverflow.com/questions/15566632/different-content-types-in-email

namespace cloudscribe.Messaging.Email.Mailgun
{
    public class MailgunEmailSender : IEmailSender
    {
        public MailgunEmailSender(
            IMailgunOptionsProvider optionsProvider,
            ILogger<MailgunEmailSender> logger
            )
        {
            _optionsProvider = optionsProvider;
            _log = logger;
        }

        private IMailgunOptionsProvider _optionsProvider;
        private ILogger _log;

        public string Name { get; } = "MailgunEmailSender";

        private MailgunOptions options = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(options == null)
            {
                options = await _optionsProvider.GetMailgunOptions(configLookupKey);
            }
            
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.DomainName))
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
            
            if (!isConfigured)
            {
                _log.LogError($"failed to send email with subject {subject} because mailgun api key or domain is empty or not configured");
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
            
            var sender = new MailgunSender(
                options.DomainName, // Mailgun Domain
                "key-" + options.ApiKey // Mailgun API Key
                    );

            var fromEmailToUse = fromEmail;
            var fromAliasToUse = fromName;
            if(string.IsNullOrWhiteSpace(fromEmailToUse))
            {
                fromEmailToUse = options.DefaultEmailFromAddress;
            }
            if (string.IsNullOrWhiteSpace(fromAliasToUse))
            {
                fromAliasToUse = options.DefaultEmailFromAlias;
            }
            
            var email = FluentEmail.Core.Email
                .From(fromEmailToUse, fromAliasToUse)
                .ReplyTo(replyToEmail, replyToName)
                .Subject(subject)
                .Body(htmlMessage, true)
                .PlaintextAlternativeBody(plainTextMessage)
                ;
            
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
                        if (!string.IsNullOrEmpty(adrs[i]))
                        {
                            email.To(adrs[i].Trim(), toAliases[i].Trim());
                        }
                    }
                }
                else
                {
                    foreach (string item in adrs)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            email.To(item.Trim());
                        }
                    }
                }
            }
            else
            {
                //not really a csv
                email.To(toEmailCsv, toAliasCsv);
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
                            if (!string.IsNullOrEmpty(adrs[i]))
                            {
                                email.CC(adrs[i].Trim(), aliases[i].Trim());
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                email.CC(item.Trim());
                            }
                        }
                    }
                }
                else
                {
                    email.CC(ccEmailCsv, ccAliasCsv);
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
                            if (!string.IsNullOrEmpty(adrs[i]))
                            {
                                email.BCC(adrs[i].Trim(), aliases[i].Trim());
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in adrs)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                email.BCC(item.Trim());
                            }
                        }
                    }
                }
                else
                {
                    email.BCC(bccEmailCsv, bccAliasCsv);
                }
            }

            if (importance == Importance.High)
            {
                email.HighPriority();
            }
            if (importance == Importance.Low)
            {
                email.LowPriority();
            }

            if (attachmentFilePaths != null && attachmentFilePaths.Length > 0)
            {
                foreach (var filePath in attachmentFilePaths)
                {
                    try
                    {
                        email.AttachFromFilename(filePath);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"failed to add attachment with path {filePath}, error was {ex.Message} : {ex.StackTrace}");
                    }
                }
            }


            email.Sender = sender;

            try
            {
                var response = await email.SendAsync();
                if (!response.Successful)
                {
                    foreach (var m in response.ErrorMessages)
                    {
                        _log.LogError($"failed to send message with subject {subject} error messages include {m}");
                    }
                }
            }
            catch(Exception ex)
            {
                _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
            }

           

        }

    }
}
