// Copyright (c) Idox Software Ltd. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts
// Created:					2023-04-19
// Last Modified:			2023-04-19
//

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using cloudscribe.Email.Senders;
using cloudscribe.Core.Storage.UserInteractiveServiceTokens;

//TODO: not sure but we may need some changes in this class to support some languages
// open to a pull request
//https://stackoverflow.com/questions/7266935/how-to-send-utf-8-email
//https://stackoverflow.com/questions/15566632/different-content-types-in-email
//http://www.mimekit.net/docs/html/T_MimeKit_TextPart.htm
//http://www.mimekit.net/docs/html/M_MimeKit_TextPart_SetText_1.htm

namespace cloudscribe.Email.SmtpOAuth
{
    public class SmtpOAuthEmailSender : IEmailSender
    {
        public SmtpOAuthEmailSender(
            ISmtpOAuthOptionsProvider smtpOAuthOptionsProvider,
            IServiceClientProvider httpClientFactory,
            ILogger<SmtpOAuthEmailSender> logger,
            IUserInteractiveServiceTokensProvider userISTokensProvider
            )
        {
            _smtpOAuthOptionsProvider = smtpOAuthOptionsProvider;
            _httpClientFactory = httpClientFactory;
            _log = logger;
            _userISTokensProvider = userISTokensProvider;
        }

        private ISmtpOAuthOptionsProvider _smtpOAuthOptionsProvider;
        private IServiceClientProvider _httpClientFactory;
        private ILogger _log;

        private IUserInteractiveServiceTokensProvider _userISTokensProvider;

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

        public string Name { get; } = "SmtpOAuthMailSender";

        private SmtpOAuthOptions smtpOAuthOptions = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(smtpOAuthOptions == null)
            {
                smtpOAuthOptions = await _smtpOAuthOptionsProvider.GetSmtpOAuthOptions(configLookupKey).ConfigureAwait(false);
            }

            if (smtpOAuthOptions == null) return false;
            //hopefully this is the tenant/siteId, which we need to get the right token from the db
            smtpOAuthOptions.SiteId = configLookupKey;

            return !string.IsNullOrWhiteSpace(smtpOAuthOptions.Server);
        }

        public async Task<EmailSendResult> SendEmailAsync(
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
            List<EmailAttachment> attachments = null,
            string charsetBodyHtml = null, // not currently used in this implementation
            string charsetBodyText = null, //not currently used in this implementation
            string configLookupKey = null
            )
        {
            var isConfigured = await IsConfigured(configLookupKey);

            if (!isConfigured)
            {
                var message = $"failed to send email with subject {subject} because smtp options are not configured";
                _log.LogError(message);
                return new EmailSendResult(false, message);
            }

            string oAuthToken = null;
            if(Guid.TryParse(smtpOAuthOptions.SiteId, out Guid siteIdGuid))
            {
                var userPrincipalName = smtpOAuthOptions.User;
                var cloudscribeServiceProvider = "SmtpOauth";
                //get authorize token here
                oAuthToken = await _userISTokensProvider.GetAccessTokenAsync(
                    siteIdGuid,
                    userPrincipalName,
                    cloudscribeServiceProvider,
                    smtpOAuthOptions.TokenEndpoint,
                    smtpOAuthOptions.ClientId,
                    smtpOAuthOptions.ClientSecret
                 );
            }

            if (string.IsNullOrWhiteSpace(oAuthToken))
            {
                var message = $"failed to send email with subject {subject} because an oauth access token could not be obtained";
                _log.LogError(message);
                return new EmailSendResult(false, message);
            }


            if (string.IsNullOrWhiteSpace(toEmailCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(fromEmail) && string.IsNullOrWhiteSpace(smtpOAuthOptions.DefaultEmailFromAddress))
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
                m.From.Add(new MailboxAddress(smtpOAuthOptions.DefaultEmailFromAlias, smtpOAuthOptions.DefaultEmailFromAddress));
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
                    if(ccAliasCsv == null) { ccAliasCsv = string.Empty; }
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
                    if(bccAliasCsv == null) { bccAliasCsv = string.Empty; }
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

            if(attachments != null && attachments.Count > 0)
            {
                foreach(var attachment in attachments)
                {
                    try
                    {
                        using (attachment.Stream)
                        {
                            var bytes = attachment.Stream.ToByteArray();
                            bodyBuilder.Attachments.Add(Path.GetFileName(attachment.FileName), bytes);
                        }

                    }
                    catch(Exception ex)
                    {
                        _log.LogError($"failed to add attachment {attachment.FileName}, error was {ex.Message} : {ex.StackTrace}");
                    }


                }
            }

            m.Body = bodyBuilder.ToMessageBody();
            try
            {
                var oAuth2 = new SaslMechanismOAuth2(smtpOAuthOptions.User, oAuthToken);
                using (var client = new SmtpClient())
                {
                    if(smtpOAuthOptions.UseSsl)
                    {
                        if (smtpOAuthOptions.Port == 587)
                            {
                                //See https://github.com/cloudscribe/cloudscribe/issues/755
                                await client.ConnectAsync(
                                    smtpOAuthOptions.Server,
                                    smtpOAuthOptions.Port,
                                    SecureSocketOptions.StartTls
                                    ).ConfigureAwait(false);
                        }
                        else
                        {
                            await client.ConnectAsync(
                                smtpOAuthOptions.Server,
                                smtpOAuthOptions.Port,
                                smtpOAuthOptions.UseSsl).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        await client.ConnectAsync(
                            smtpOAuthOptions.Server,
                            smtpOAuthOptions.Port,
                            smtpOAuthOptions.UseSsl).ConfigureAwait(false);
                    }
                    await client.AuthenticateAsync(oAuth2);
                    await client.SendAsync(m);
                    await client.DisconnectAsync(true);
                }
            }
            catch(Exception ex)
            {
                var message = $"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}";
                _log.LogError(message);
                return new EmailSendResult(false, message);
            }

            return new EmailSendResult(true);
        }
    }
}
