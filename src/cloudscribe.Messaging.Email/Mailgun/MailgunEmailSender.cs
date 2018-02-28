using FluentEmail.Core.Models;
using FluentEmail.Mailgun;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

//https://elanderson.net/2017/02/email-with-asp-net-core-using-mailgun/
//https://github.com/lukencode/FluentEmail/blob/master/src/Senders/FluentEmail.Mailgun/MailgunSender.cs

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

        public async Task<bool> IsConfigured()
        {
            var options = await _optionsProvider.GetMailgunOptions();
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
            string[] attachmentFilePaths = null
            )
        {
            var options = await _optionsProvider.GetMailgunOptions();
            if (options == null || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.DomainName))
            {
                _log.LogError($"failed to send email with subject {subject} because mailgun api key or domain is empty or not configured");

                return;
            }

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


            var sender = new MailgunSender(
                options.DomainName, // Mailgun Domain
                "key-" + options.ApiKey // Mailgun API Key
                    );

            var email = FluentEmail.Core.Email
                .From(fromEmail, fromName)
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

            var response = await email.SendAsync();
            if (!response.Successful)
            {
                foreach (var m in response.ErrorMessages)
                {
                    _log.LogError($"failed to send message with subject {subject} error messages include {m}");
                }
            }


        }

    }
}
