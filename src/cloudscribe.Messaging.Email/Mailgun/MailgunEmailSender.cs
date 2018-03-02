// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-28
// Last Modified:			2018-03-02
// 


using FluentEmail.Mailgun;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.EndpointUrl))
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

            
            if (attachmentFilePaths == null || attachmentFilePaths.Length == 0)
            {
                await SendWithoutAttachments(keyValues, options, subject);
            }
            else
            {
                //https://stackoverflow.com/questions/29407791/email-attachments-in-httpclient

                var filesStream = new List<Stream>();
                var fileNames = new List<string>();
                foreach (var filePath in attachmentFilePaths)
                {
                    try
                    {
                        var file = File.OpenRead(filePath);
                        filesStream.Add(file);
                        fileNames.Add(Path.GetFileName(filePath));
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"failed to add attachment with path {filePath}, error was {ex.Message} : {ex.StackTrace}");
                    }
                }

                await SendWithAttachments(keyValues, options, subject, filesStream.ToArray(), fileNames.ToArray());

            }

        }

        private async Task SendWithoutAttachments(List<KeyValuePair<string, string>> keyValues, MailgunOptions options, string subject)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(options.ApiKey)));

                var content = new FormUrlEncodedContent(keyValues);

                try
                {
                    var response = await client.PostAsync(options.EndpointUrl, content).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        _log.LogError($"failed to send email with subject {subject} error was {response.StatusCode} : {result}");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
                }

            }


        }

        public async Task SendWithAttachments(List<KeyValuePair<string, string>> keyValues, MailgunOptions options, string subject, Stream[] paramFileStream = null, string[] filenames = null)
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(options.ApiKey)));

                foreach (var item in keyValues)
                {
                    HttpContent stringContent = new StringContent(item.Value);
                    formData.Add(stringContent, item.Key);
                }

                for (int i = 0; i < paramFileStream.Length; i++)
                {
                    HttpContent fileStreamContent = new StreamContent(paramFileStream[i]);
                    formData.Add(fileStreamContent, "file" + i, filenames[i]);
                }

                try
                {
                    var response = await client.PostAsync(options.EndpointUrl, formData);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        _log.LogError($"failed to send email with subject {subject} error was {response.StatusCode} : {result}");
                    }


                }
                catch (Exception ex)
                {
                    _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
                }


            }
        }


        //public async Task SendEmailAsync(
        //    string toEmailCsv,
        //    string fromEmail,
        //    string subject,
        //    string plainTextMessage,
        //    string htmlMessage,
        //    string replyToEmail = null,
        //    Importance importance = Importance.Normal,
        //    bool isTransactional = true,
        //    string fromName = null,
        //    string replyToName = null,
        //    string toAliasCsv = null,
        //    string ccEmailCsv = null,
        //    string ccAliasCsv = null,
        //    string bccEmailCsv = null,
        //    string bccAliasCsv = null,
        //    string[] attachmentFilePaths = null,
        //    string charsetBodyHtml = null,
        //    string charsetBodyText = null,
        //    string configLookupKey = null
        //    )
        //{
        //    var isConfigured = await IsConfigured(configLookupKey);

        //    if (!isConfigured)
        //    {
        //        _log.LogError($"failed to send email with subject {subject} because mailgun api key or domain is empty or not configured");
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(toEmailCsv))
        //    {
        //        throw new ArgumentException("no to addresses provided");
        //    }

        //    if (string.IsNullOrWhiteSpace(fromEmail) && string.IsNullOrWhiteSpace(options.DefaultEmailFromAddress))
        //    {
        //        throw new ArgumentException("no from address provided");
        //    }

        //    if (string.IsNullOrWhiteSpace(subject))
        //    {
        //        throw new ArgumentException("no subject provided");
        //    }

        //    var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
        //    var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
        //    if (!hasPlainText && !hasHtml)
        //    {
        //        throw new ArgumentException("no message provided");
        //    }

        //    var sender = new MailgunSender(
        //        options.EndpointUrl, // Mailgun Domain
        //        options.ApiKey // Mailgun API Key
        //            );

        //    var fromEmailToUse = fromEmail;
        //    var fromAliasToUse = fromName;
        //    if(string.IsNullOrWhiteSpace(fromEmailToUse))
        //    {
        //        fromEmailToUse = options.DefaultEmailFromAddress;
        //    }
        //    if (string.IsNullOrWhiteSpace(fromAliasToUse))
        //    {
        //        fromAliasToUse = options.DefaultEmailFromAlias;
        //    }

        //    var email = FluentEmail.Core.Email
        //        .From(fromEmailToUse, fromAliasToUse)
        //        .ReplyTo(replyToEmail, replyToName)
        //        .Subject(subject)
        //        .Body(htmlMessage, true)
        //        .PlaintextAlternativeBody(plainTextMessage)
        //        ;

        //    if (toEmailCsv.Contains(","))
        //    {
        //        var useToAliases = false;
        //        string[] adrs = toEmailCsv.Split(',');
        //        string[] toAliases = new string[0];
        //        if (toAliasCsv != null && toAliasCsv.Contains(","))
        //        {
        //            toAliases = toAliasCsv.Split(',');
        //            if (toAliases.Length > 0 && toAliases.Length == adrs.Length)
        //            {
        //                useToAliases = true;
        //            }
        //        }
        //        if (useToAliases)
        //        {
        //            for (int i = 0; i < adrs.Length; i++)
        //            {
        //                if (!string.IsNullOrEmpty(adrs[i]))
        //                {
        //                    email.To(adrs[i].Trim(), toAliases[i].Trim());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (string item in adrs)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                {
        //                    email.To(item.Trim());
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //not really a csv
        //        email.To(toEmailCsv, toAliasCsv);
        //    }

        //    if (!string.IsNullOrWhiteSpace(ccEmailCsv))
        //    {
        //        if (ccEmailCsv.Contains(","))
        //        {
        //            var useAliases = false;
        //            string[] adrs = ccEmailCsv.Split(',');
        //            string[] aliases = new string[0];
        //            if (ccAliasCsv != null && ccAliasCsv.Contains(","))
        //            {
        //                aliases = ccAliasCsv.Split(',');
        //                if (aliases.Length > 0 && aliases.Length == adrs.Length)
        //                {
        //                    useAliases = true;
        //                }
        //            }
        //            if (useAliases)
        //            {
        //                for (int i = 0; i < adrs.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(adrs[i]))
        //                    {
        //                        email.CC(adrs[i].Trim(), aliases[i].Trim());
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (string item in adrs)
        //                {
        //                    if (!string.IsNullOrEmpty(item))
        //                    {
        //                        email.CC(item.Trim());
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            email.CC(ccEmailCsv, ccAliasCsv);
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(bccEmailCsv))
        //    {
        //        if (bccEmailCsv.Contains(","))
        //        {
        //            var useAliases = false;
        //            string[] adrs = bccEmailCsv.Split(',');
        //            string[] aliases = new string[0];
        //            if (bccAliasCsv != null && bccAliasCsv.Contains(","))
        //            {
        //                aliases = bccAliasCsv.Split(',');
        //                if (aliases.Length > 0 && aliases.Length == adrs.Length)
        //                {
        //                    useAliases = true;
        //                }
        //            }
        //            if (useAliases)
        //            {
        //                for (int i = 0; i < adrs.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(adrs[i]))
        //                    {
        //                        email.BCC(adrs[i].Trim(), aliases[i].Trim());
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (string item in adrs)
        //                {
        //                    if (!string.IsNullOrEmpty(item))
        //                    {
        //                        email.BCC(item.Trim());
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            email.BCC(bccEmailCsv, bccAliasCsv);
        //        }
        //    }

        //    if (importance == Importance.High)
        //    {
        //        email.HighPriority();
        //    }
        //    if (importance == Importance.Low)
        //    {
        //        email.LowPriority();
        //    }

        //    if (attachmentFilePaths != null && attachmentFilePaths.Length > 0)
        //    {
        //        foreach (var filePath in attachmentFilePaths)
        //        {
        //            try
        //            {
        //                email.AttachFromFilename(filePath);
        //            }
        //            catch (Exception ex)
        //            {
        //                _log.LogError($"failed to add attachment with path {filePath}, error was {ex.Message} : {ex.StackTrace}");
        //            }
        //        }
        //    }


        //    email.Sender = sender;

        //    try
        //    {
        //        var response = await email.SendAsync();
        //        if (!response.Successful)
        //        {
        //            foreach (var m in response.ErrorMessages)
        //            {
        //                _log.LogError($"failed to send message with subject {subject} error messages include {m}");
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.LogError($"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}");
        //    }



        //}

    }
}
