// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-03-01
// Last Modified:			2018-03-06
// 

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace cloudscribe.Email.ElasticEmail
{
    public class ElasticEmailSender : IEmailSender
    {
        public ElasticEmailSender(
            IElasticEmailOptionsProvider optionsProvider,
            ILogger<ElasticEmailSender> logger
            )
        {
            _optionsProvider = optionsProvider;
            _log = logger;
        }

        private IElasticEmailOptionsProvider _optionsProvider;
        private ILogger _log;

        private const string apiEndpoint = "https://api.elasticemail.com/v2";

        public string Name { get; } = "ElasticEmailSender";

        private ElasticEmailOptions options = null;
        public async Task<bool> IsConfigured(string configLookupKey = null)
        {
            if(options == null)
            {
                options = await _optionsProvider.GetElasticEmailOptions(configLookupKey);
            }
            
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey))
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
                var message = $"failed to send email with subject {subject} because elasticemail api key is empty or not configured";
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

            keyValues.Add(new KeyValuePair<string, string>("apikey", options.ApiKey));

            if (!string.IsNullOrWhiteSpace(fromEmail))
            {
                keyValues.Add(new KeyValuePair<string, string>("from", fromEmail));
                if (!string.IsNullOrWhiteSpace(fromName))
                {
                    keyValues.Add(new KeyValuePair<string, string>("fromName", fromName));
                }
            }
            else
            {
                keyValues.Add(new KeyValuePair<string, string>("from", options.DefaultEmailFromAddress));
                if (!string.IsNullOrWhiteSpace(options.DefaultEmailFromAlias))
                {
                    keyValues.Add(new KeyValuePair<string, string>("fromName", options.DefaultEmailFromAlias));
                }
            }
            
            if (!string.IsNullOrWhiteSpace(replyToEmail))
            {
                keyValues.Add(new KeyValuePair<string, string>("replyTo", replyToEmail));
            }
            if (!string.IsNullOrWhiteSpace(replyToName))
            {
                keyValues.Add(new KeyValuePair<string, string>("replyToName", replyToName));
            }

            keyValues.Add(new KeyValuePair<string, string>("subject", subject));
            if(!string.IsNullOrWhiteSpace(htmlMessage))
            {
                keyValues.Add(new KeyValuePair<string, string>("bodyHtml", htmlMessage));
            }

            if (!string.IsNullOrWhiteSpace(plainTextMessage))
            {
                keyValues.Add(new KeyValuePair<string, string>("bodyText", plainTextMessage));
            }

            if(!string.IsNullOrWhiteSpace(charsetBodyHtml))
            {
                keyValues.Add(new KeyValuePair<string, string>("charsetBodyHtml", charsetBodyHtml));
            }
            if (!string.IsNullOrWhiteSpace(charsetBodyText))
            {
                keyValues.Add(new KeyValuePair<string, string>("charsetBodyText", charsetBodyText));
            }

            keyValues.Add(new KeyValuePair<string, string>("isTransactional", isTransactional.ToString().ToLower()));
                
            keyValues.Add(new KeyValuePair<string, string>("msgTo", toEmailCsv));
            if(!string.IsNullOrWhiteSpace(ccEmailCsv))
            {
                keyValues.Add(new KeyValuePair<string, string>("msgCC", ccEmailCsv));
            }
            if (!string.IsNullOrWhiteSpace(bccEmailCsv))
            {
                keyValues.Add(new KeyValuePair<string, string>("msgBcc", bccEmailCsv));
            }

            if(attachments == null || attachments.Count == 0)
            {
                return await SendWithoutAttachments(keyValues, options, subject);
            }
            else
            {
                return await SendWithAttachments(keyValues, options, subject, attachments);
            }  
        }

        private async Task<EmailSendResult> SendWithoutAttachments(List<KeyValuePair<string, string>> keyValues, ElasticEmailOptions options, string subject)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(keyValues);
                var endpoint = apiEndpoint;
                if(!string.IsNullOrWhiteSpace(options.EndpointUrl))
                {
                    endpoint = options.EndpointUrl;
                }

                try
                {
                    var response = await client.PostAsync(endpoint + "/email/send", content).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode || result.Contains("Oops"))
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

        public async Task<EmailSendResult> SendWithAttachments(
            List<KeyValuePair<string, string>> keyValues, 
            ElasticEmailOptions options, 
            string subject,
            List<EmailAttachment> attachments)
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                foreach (var item in keyValues)
                {
                    HttpContent stringContent = new StringContent(item.Value);
                    formData.Add(stringContent, item.Key);
                }

                int i = 0;
                foreach(var attachement in attachments)
                {
                    HttpContent fileStreamContent = new StreamContent(attachement.Stream);
                    formData.Add(fileStreamContent, "file" + i, attachement.FileName);
                    i++;
                }

                try
                {
                    var endpoint = apiEndpoint;
                    if (!string.IsNullOrWhiteSpace(options.EndpointUrl))
                    {
                        endpoint = options.EndpointUrl;
                    }

                    var response = await client.PostAsync(endpoint + "/email/send", formData);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode || result.Contains("Oops"))
                    {
                        var message = $"failed to send email with subject {subject} error was {response.StatusCode} : {result}";
                        _log.LogError(message);
                        return new EmailSendResult(false, message);
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
}
