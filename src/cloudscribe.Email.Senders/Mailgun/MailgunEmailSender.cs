// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-28
// Last Modified:			2018-05-06
// 


using cloudscribe.Email.Senders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Email.Mailgun
{
    public class MailgunEmailSender : IEmailSender
    {
        public MailgunEmailSender(
            IMailgunOptionsProvider optionsProvider,
            IServiceClientProvider httpClientFactory,
            ILogger<MailgunEmailSender> logger
            )
        {
            _optionsProvider = optionsProvider;
            _httpClientFactory = httpClientFactory;
            _log = logger;
        }

        private IMailgunOptionsProvider _optionsProvider;
        private IServiceClientProvider _httpClientFactory;
        private ILogger _log;
        private const string apiBaseUrl = "https://api.mailgun.net/v3";

        public string Name { get; } = "MailgunEmailSender";

        private MailgunOptions options = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(options == null)
            {
                options = await _optionsProvider.GetMailgunOptions(configLookupKey);
            }
            
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.EndpointUrl))
            {
                return false;
            }
            return true;

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
            string charsetBodyHtml = null,
            string charsetBodyText = null,
            string configLookupKey = null
            )
        {
            var isConfigured = await IsConfigured(configLookupKey);

            if (!isConfigured)
            {
                var message = $"failed to send email with subject {subject} because mailgun api key or domain is empty or not configured";
                _log.LogError(message);
                return new EmailSendResult(false, message);
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


#pragma warning disable IDE0028 // Simplify collection initialization
            var keyValues = new List<KeyValuePair<string, string>>();
#pragma warning restore IDE0028 // Simplify collection initialization

            //"Bob <bob@host.com>"
            var emailFormat = "{0} <{1}>";
            if (!string.IsNullOrWhiteSpace(fromEmail))
            {
                
                if (!string.IsNullOrWhiteSpace(fromName))
                {
                    keyValues.Add(new KeyValuePair<string, string>("from", string.Format(emailFormat, fromName, fromEmail)));
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("from", fromEmail));
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(options.DefaultEmailFromAlias))
                {
                    keyValues.Add(new KeyValuePair<string, string>("from", string.Format(emailFormat, options.DefaultEmailFromAlias, options.DefaultEmailFromAddress)));
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("from", options.DefaultEmailFromAddress));
                }
            }

            if (!string.IsNullOrWhiteSpace(replyToEmail))
            {
                if (!string.IsNullOrWhiteSpace(replyToName))
                {
                    keyValues.Add(new KeyValuePair<string, string>("replyToName", string.Format(emailFormat,replyToName, replyToEmail)));
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("replyTo", replyToEmail));
                }
                
            }
            
            keyValues.Add(new KeyValuePair<string, string>("subject", subject));
            if (!string.IsNullOrWhiteSpace(htmlMessage))
            {
                keyValues.Add(new KeyValuePair<string, string>("html", htmlMessage));
            }

            if (!string.IsNullOrWhiteSpace(plainTextMessage))
            {
                keyValues.Add(new KeyValuePair<string, string>("text", plainTextMessage));
            }
            
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
                    var sb = new StringBuilder();
                    var comma = "";
                    for (int i = 0; i < adrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(adrs[i]))
                        {
                            sb.Append(comma + string.Format(emailFormat, toAliases[i].Trim(), adrs[i].Trim()));
                            //email.To(adrs[i].Trim(), toAliases[i].Trim());
                            comma = ",";
                        }    
                    }
                    keyValues.Add(new KeyValuePair<string, string>("to", sb.ToString()));
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("to", toEmailCsv));
                }
            }
            else
            {
                //not really a csv
                keyValues.Add(new KeyValuePair<string, string>("to", toEmailCsv));
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
                        var sb = new StringBuilder();
                        var comma = "";
                        for (int i = 0; i < adrs.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(adrs[i]))
                            {
                                sb.Append(comma + string.Format(emailFormat, aliases[i].Trim(), adrs[i].Trim()));
                                comma = ",";
                            }
                        }
                        keyValues.Add(new KeyValuePair<string, string>("cc", sb.ToString()));
                    }
                    else
                    {
                        keyValues.Add(new KeyValuePair<string, string>("cc", ccEmailCsv));
                    }
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("cc", ccEmailCsv));
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
                        var sb = new StringBuilder();
                        var comma = "";
                        for (int i = 0; i < adrs.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(adrs[i]))
                            {
                                sb.Append(comma + string.Format(emailFormat, aliases[i].Trim(), adrs[i].Trim()));
                                comma = ",";
                            }
                        }
                        keyValues.Add(new KeyValuePair<string, string>("bcc", sb.ToString()));
                    }
                    else
                    {
                        keyValues.Add(new KeyValuePair<string, string>("bcc", bccEmailCsv));
                    }
                }
                else
                {
                    keyValues.Add(new KeyValuePair<string, string>("bcc", bccEmailCsv));
                }
            }

            //if (importance == Importance.High)
            //{
            //    email.HighPriority();
            //}
            //if (importance == Importance.Low)
            //{
            //    email.LowPriority();
            //}

            
            if (attachments == null || attachments.Count == 0)
            {
                return await SendWithoutAttachments(keyValues, options, subject);
            }
            else
            {
                var files = new List<ByteArrayContent>();
                foreach (var attachment in attachments)
                {
                    try
                    {
                        var fileContent = new ByteArrayContent(attachment.Stream.ToByteArray());

                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "attachment",
                            FileName = Path.GetFileName(attachment.FileName)
                        };

                        files.Add(fileContent);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"failed to add attachment with path {attachment.FileName}, error was {ex.Message} : {ex.StackTrace}");
                    }
                }

                return await SendWithAttachments(keyValues, options, subject, files);

            }

        }

        private string PrefixApiKey(string apiKey)
        {
            if(!string.IsNullOrWhiteSpace(apiKey))
            {
                if(!apiKey.StartsWith("api:"))
                {
                    return "api:" + apiKey;
                }
            }

            return apiKey;
        }

        private async Task<EmailSendResult> SendWithoutAttachments(List<KeyValuePair<string, string>> keyValues, MailgunOptions options, string subject)
        {
            var client = _httpClientFactory.GetOrCreateHttpClient(new Uri(options.EndpointUrl));
            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(PrefixApiKey(options.ApiKey))));
                
            var content = new FormUrlEncodedContent(keyValues);

            try
            {
                var response = await client.PostAsync( "messages", content).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"failed to send email with subject {subject} error was {response.StatusCode} : {result}";
                    _log.LogError(message);
                    return new EmailSendResult(false, message);
                }
            }
            catch (Exception ex)
            {
                var message = $"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}";
                _log.LogError(message);
                return new EmailSendResult(false, message);
            }
            
            return new EmailSendResult(true);
        }

        public async Task<EmailSendResult> SendWithAttachments(
            List<KeyValuePair<string, string>> keyValues, 
            MailgunOptions options, 
            string subject,
            List<ByteArrayContent> files
            )
        {
            var client = _httpClientFactory.GetOrCreateHttpClient(new Uri(options.EndpointUrl));
            using (var formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(PrefixApiKey(options.ApiKey))));

                foreach (var item in keyValues)
                {
                    HttpContent stringContent = new StringContent(item.Value);
                    formData.Add(stringContent, item.Key);
                }

                foreach (var file in files)
                {
                    formData.Add(file);
                }

                try
                {
                    var response = await client.PostAsync("messages", formData);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        var message = $"failed to send email with subject {subject} error was {response.StatusCode} : {result}";
                        _log.LogError(message);
                        return new EmailSendResult(false, message);
                    }


                }
                catch (Exception ex)
                {
                    var message = $"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}";
                    _log.LogError(message);
                    return new EmailSendResult(false, message);
                }

                return new EmailSendResult(true);
            }
        }


        
    }
}
