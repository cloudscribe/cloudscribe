// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-02-27
// Last Modified:			2018-03-06
// 

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

//TODO: not sure but we may need some changes in this class to support some languages
// open to a pull request
//https://stackoverflow.com/questions/7266935/how-to-send-utf-8-email
//https://stackoverflow.com/questions/15566632/different-content-types-in-email
//http://www.mimekit.net/docs/html/T_MimeKit_TextPart.htm
//http://www.mimekit.net/docs/html/M_MimeKit_TextPart_SetText_1.htm

namespace cloudscribe.Email.Smtp
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
            if (smtpOptions == null)
            {
                smtpOptions = await _smtpOptionsProvider.GetSmtpOptions(configLookupKey).ConfigureAwait(false);
            }

            if (smtpOptions == null) return false;

            return !string.IsNullOrWhiteSpace(smtpOptions.Server);
        }


        /// <summary>
        /// ////////////////////////////////////////////////


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

            /*
             From https://github.com/jstedfast/MailKit/blob/master/ExchangeOAuth2.md 
           

            Anyhow - always getting errors:
              Only http uri scheme is supported, but https was found. 
            Configure http://localhost or http://localhost:port both during app registration and when you create 
            the PublicClientApplication object. See https://aka.ms/msal-net-os-browser for details
  

            Only loopback redirect uri is supported, but https://workflowplatform.esdm.co.uk/signin-oidc was found. 
            Configure http://localhost or http://localhost:port both during app registration and when you create the 
            PublicClientApplication object. See https://aka.ms/msal-net-os-browser for details
             
             */
            // TRY USING OLD CREDS FOR SMTP HERE

            var options = new PublicClientApplicationOptions
            {
                ClientId = "55cb2bc7-365e-4637-98b5-f5f5fe7ca13d", //   jim2:  "bd2f24e5-1e8e-4dfe-820f-0c7ee63f6fa8",   // IWP: "d85d494e-cc03-44a5-8317-06542e8d6944",   //  jim_reluc:  "55cb2bc7-365e-4637-98b5-f5f5fe7ca13d",
                TenantId = "d5fe733d-6d61-4060-a35c-27f589675587", 
                // RedirectUri = "https://workflowplatform.esdm.co.uk/signin-oidc"
               // RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient"
               
               
            };

            var publicClientApplication = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(options)
                .WithRedirectUri("http://localhost:44399/signin-oidc")
                .Build();

            var scopes = new string[] {
               "https://graph.microsoft.com/User.Read"
              //  "email",
             //   "offline_access",
                          // "https://outlook.office.com/IMAP.AccessAsUser.All", // Only needed for IMAP
                         // "https://outlook.office.com/POP.AccessAsUser.All",  // Only needed for POP
               // "https://outlook.office.com/SMTP.Send" // Only needed for SMTP
               // "openid"
            };

            try
            {
                _log.LogError("going for authToken");


                                /*  confidential route gets us a token 
                                 *  
                                // nCy8Q~WN1a7Eo1wBw-v3l-b.25Vdg.NzHrJuSbgx
                
                                var cco = new ConfidentialClientApplicationOptions
                                {
                                    ClientId = "55cb2bc7-365e-4637-98b5-f5f5fe7ca13d",
                                    TenantId = "d5fe733d-6d61-4060-a35c-27f589675587",
                                    ClientSecret = "nCy8Q~WN1a7Eo1wBw-v3l-b.25Vdg.NzHrJuSbgx"
                                };

                                var cc = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(cco)
                                .WithRedirectUri("http://localhost:44399/signin-oidc")
                                .Build();

                                string[] ccscope = new string[] { "https://graph.microsoft.com/.default" };

                                var authToken = await cc.AcquireTokenForClient(ccscope).ExecuteAsync();

                                */


                var builder = publicClientApplication.AcquireTokenByUsernamePassword(scopes, "NoReply@esdm.co.uk", "tROtkF0Wt1Hi");
                var authToken = await builder.ExecuteAsync();
                // var authToken = await publicClientApplication.AcquireTokenInteractive(scopes).ExecuteAsync();


                /* 
                 * At present:

                 AADSTS65001: The user or administrator has not consented to use the application with ID 'bd2f24e5-1e8e-4dfe-820f-0c7ee63f6fa8' named 'jim2'. 
                 Send an interactive authorization request for this user and resource.
                Trace ID: 36eaabac-2c93-4942-a553-8c5e794a1d00
                Correlation ID: f0aacd6b-999b-46a1-ab78-697880fa1ae5
                Timestamp: 2022-08-30 15:14:52Z


                Suggest maybe here:
                https://portal.azure.com/#view/Microsoft_AAD_IAM/ManagedAppMenuBlade/~/Permissions/objectId/aae51288-adf1-490e-a469-8ef6662d5173/appId/bd2f24e5-1e8e-4dfe-820f-0c7ee63f6fa8/menuItemId/Permissions  


                See also
                https://stackoverflow.com/questions/67520483/mailkit-cant-authenticate-with-o365-oauth2-account
                
                It appears that OAUTH2 authentication with Office365 via the non-interactive method is unsupported by the Microsoft Exchange IMAP/POP3/SMTP protocols and that the only way to get access to Office365 mail using the non-interactive method of OAUTH2 authentication is via the Microsoft.Graph API.

                I've been getting a lot of questions about this over the past few months and as far as I'm aware, no one (myself included) has been able to find a way to make this work.

                I keep hoping to see someone trying to do this (even in another language) here on StackOverflow with an accepted answer. So far, all I've seen are questions about OAuth2 using the interactive approach (which, as you've seen, I have written documentation for and is known to work well with MailKit).

                 */



                _log.LogError("got authToken");

                var oauth2 = new SaslMechanismOAuth2(authToken.Account.Username, authToken.AccessToken);
                //var oauth2 = new SaslMechanismOAuth2("JimKerslake@esdm.co.uk", authToken.AccessToken);


                 _log.LogError("making client for " + authToken.Account.Username);

                using (var client = new SmtpClient())
                {
                    //await client.ConnectAsync("outlook.office365.com", 993, SecureSocketOptions.SslOnConnect);
                    await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(oauth2);
                    await client.DisconnectAsync(true);
                }

                _log.LogError("Success");
            }

            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);

                var s = 1;
            }

            return new EmailSendResult(true);

        }


        /// 
        /// /////////////////////////////////////////////////


        //public async Task<EmailSendResult> SendEmailAsync(
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
        //    List<EmailAttachment> attachments = null,
        //    string charsetBodyHtml = null, // not currently used in this implementation
        //    string charsetBodyText = null, //not currently used in this implementation
        //    string configLookupKey = null
        //    )
        //{
        //    var isConfigured = await IsConfigured(configLookupKey);

        //    if (!isConfigured)
        //    {
        //        var message = $"failed to send email with subject {subject} because smtp options are not configured";
        //        _log.LogError(message);
        //        return new EmailSendResult(false, message);
        //    }

        //    if (string.IsNullOrWhiteSpace(toEmailCsv))
        //    {
        //        throw new ArgumentException("no to addresses provided");
        //    }

        //    if (string.IsNullOrWhiteSpace(fromEmail) && string.IsNullOrWhiteSpace(smtpOptions.DefaultEmailFromAddress))
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

        //    var m = new MimeMessage();
        //    if (!string.IsNullOrEmpty(fromEmail))
        //    {
        //        m.From.Add(new MailboxAddress(fromName, fromEmail));
        //    }
        //    else
        //    {
        //        m.From.Add(new MailboxAddress(smtpOptions.DefaultEmailFromAlias, smtpOptions.DefaultEmailFromAddress));
        //    }


        //    if (!string.IsNullOrWhiteSpace(replyToEmail))
        //    {
        //        m.ReplyTo.Add(new MailboxAddress(replyToName, replyToEmail));
        //    }

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
        //                if (!string.IsNullOrEmpty(adrs[i])) { m.To.Add(new MailboxAddress(toAliases[i].Trim(), adrs[i].Trim())); }
        //            }
        //        }
        //        else
        //        {
        //            foreach (string item in adrs)
        //            {
        //                if (!string.IsNullOrEmpty(item)) { m.To.Add(new MailboxAddress("", item.Trim())); }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        m.To.Add(new MailboxAddress(toAliasCsv, toEmailCsv));
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
        //                    if (!string.IsNullOrEmpty(adrs[i])) { m.Cc.Add(new MailboxAddress(aliases[i].Trim(), adrs[i].Trim())); }
        //                }
        //            }
        //            else
        //            {
        //                foreach (string item in adrs)
        //                {
        //                    if (!string.IsNullOrEmpty(item)) { m.Cc.Add(new MailboxAddress("", item.Trim())); }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (ccAliasCsv == null) { ccAliasCsv = string.Empty; }
        //            m.Cc.Add(new MailboxAddress(ccAliasCsv, ccEmailCsv));
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
        //                    if (!string.IsNullOrEmpty(adrs[i])) { m.Bcc.Add(new MailboxAddress(aliases[i].Trim(), adrs[i].Trim())); }
        //                }
        //            }
        //            else
        //            {
        //                foreach (string item in adrs)
        //                {
        //                    if (!string.IsNullOrEmpty(item)) { m.Bcc.Add(new MailboxAddress("", item.Trim())); }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (bccAliasCsv == null) { bccAliasCsv = string.Empty; }
        //            m.Bcc.Add(new MailboxAddress(bccAliasCsv, bccEmailCsv));
        //        }
        //    }


        //    m.Subject = subject;
        //    m.Importance = GetMessageImportance(importance);

        //    if (!isTransactional)
        //    {
        //        var h = new Header(HeaderId.Precedence, "bulk");
        //        m.Headers.Add(h);
        //    }

        //    var bodyBuilder = new BodyBuilder();
        //    if (hasPlainText)
        //    {
        //        bodyBuilder.TextBody = plainTextMessage;
        //    }

        //    if (hasHtml)
        //    {
        //        bodyBuilder.HtmlBody = htmlMessage;
        //    }

        //    if (attachments != null && attachments.Count > 0)
        //    {
        //        foreach (var attachment in attachments)
        //        {
        //            try
        //            {
        //                using (attachment.Stream)
        //                {
        //                    var bytes = attachment.Stream.ToByteArray();
        //                    bodyBuilder.Attachments.Add(Path.GetFileName(attachment.FileName), bytes);
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                _log.LogError($"failed to add attachment {attachment.FileName}, error was {ex.Message} : {ex.StackTrace}");
        //            }


        //        }
        //    }

        //    m.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        if (smtpOptions.UseSsl)
        //        {

        //            if (smtpOptions.Port == 587)
        //            {
        //                //See https://github.com/cloudscribe/cloudscribe/issues/755
        //                await client.ConnectAsync(
        //                    smtpOptions.Server,
        //                    smtpOptions.Port,
        //                    SecureSocketOptions.StartTls
        //                    ).ConfigureAwait(false);
        //            }
        //            else
        //            {
        //                await client.ConnectAsync(
        //                    smtpOptions.Server,
        //                    smtpOptions.Port,
        //                    smtpOptions.UseSsl).ConfigureAwait(false);
        //            }
        //        }
        //        else
        //        {
        //            await client.ConnectAsync(
        //                smtpOptions.Server,
        //                smtpOptions.Port,
        //                SecureSocketOptions.None).ConfigureAwait(false);
        //        }


        //        // Note: since we don't have an OAuth2 token, disable
        //        // the XOAUTH2 authentication mechanism.
        //        client.AuthenticationMechanisms.Remove("XOAUTH2");

        //        // Note: only needed if the SMTP server requires authentication
        //        if (smtpOptions.RequiresAuthentication)
        //        {
        //            await client.AuthenticateAsync(
        //                smtpOptions.User,
        //                smtpOptions.Password).ConfigureAwait(false);
        //        }

        //        try
        //        {
        //            await client.SendAsync(m).ConfigureAwait(false);
        //            await client.DisconnectAsync(true).ConfigureAwait(false);
        //        }
        //        catch (Exception ex)
        //        {
        //            var message = $"failed to send email with subject {subject} error was {ex.Message} : {ex.StackTrace}";
        //            _log.LogError(message);
        //            return new EmailSendResult(false, message);
        //        }

        //        return new EmailSendResult(true);

        //    }

        //}



    }
}
