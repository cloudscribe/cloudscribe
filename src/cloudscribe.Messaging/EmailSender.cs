using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Text;
using log4net;
using cloudscribe.Configuration;

namespace cloudscribe.Messaging
{
    public class EmailSender
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailSender));
        private static bool debugLog = log.IsDebugEnabled;

        public static bool Send(SmtpSettings smtpSettings, MailMessage message)
        {
            if (message.To.ToString() == "admin@admin.com") { return false; } //demo site

            // for monitoring demo site to make sure no one has hijacked it to send spam
            string globalBcc = AppSettings.GetString("GlobalBCC", string.Empty);

            if (globalBcc.Length > 0)
            {
                MailAddress bcc = new MailAddress(globalBcc);
                message.Bcc.Add(bcc);
            }

            int timeoutMilliseconds = AppSettings.GetInt("SMTPTimeoutInMilliseconds", 15000);

            using (SmtpClient smtpClient = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = smtpSettings.UseSsl;
                smtpClient.Timeout = timeoutMilliseconds;

                if (smtpSettings.RequiresAuthentication)
                {

                    NetworkCredential smtpCredential
                        = new NetworkCredential(
                            smtpSettings.User,
                            smtpSettings.Password);

                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(smtpSettings.Server, smtpSettings.Port, "LOGIN", smtpCredential);

                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = myCache;
                }
                else
                {
                    //aded 2010-01-22 JA
                    smtpClient.UseDefaultCredentials = true;
                }


                try
                {
                    smtpClient.Send(message);
                    //log.Debug("Sent Message: " + subject);
                    //log.Info("Sent Message: " + subject);

                    bool logEmail = AppSettings.GetBool("LogAllEmailsWithSubject", false);

                    if (logEmail) { log.Info("Sent message " + message.Subject + " to " + message.To[0].Address); }

                    return true;
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    //log.Error("error sending email to " + to + " from " + from, ex);
                    log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", will retry", ex);
                    //return RetrySend(message, smtpClient, ex);

                }
                catch (WebException ex)
                {
                    log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                    return false;
                }
                catch (SocketException ex)
                {
                    log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                    return false;
                }
                catch (FormatException ex)
                {
                    log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                    return false;
                }

                return false;

            } // end using smtp client

        }

        public static void SetMessageEncoding(SmtpSettings smtpSettings, MailMessage mail)
        {
            //http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx

            if (smtpSettings.PreferredEncoding.Length > 0)
            {
                switch (smtpSettings.PreferredEncoding)
                {
                    case "ascii":
                    case "us-ascii":
                        // do nothing since this is the default
                        break;

                    case "utf32":
                    case "utf-32":

                        mail.BodyEncoding = Encoding.UTF32;
                        mail.SubjectEncoding = Encoding.UTF32;

                        break;

                    case "unicode":

                        mail.BodyEncoding = Encoding.Unicode;
                        mail.SubjectEncoding = Encoding.Unicode;

                        break;

                    case "utf8":
                    case "utf-8":

                        mail.BodyEncoding = Encoding.UTF8;
                        mail.SubjectEncoding = Encoding.UTF8;

                        break;

                    default:

                        try
                        {
                            mail.BodyEncoding = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                            mail.SubjectEncoding = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                        }
                        catch (ArgumentException ex)
                        {
                            log.Error(ex);
                        }

                        break;
                }

            }

        }

    }
}
