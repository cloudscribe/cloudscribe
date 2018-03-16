using System.Collections.Generic;
using System.Threading.Tasks;



namespace cloudscribe.Email
{
    public interface IEmailSender
    {
        string Name { get; }

        Task<bool> IsConfigured(string configLookupKey = null);

       /// <summary>
       /// A common interface for sending email with implementations available for common email services and smtp
       /// </summary>
       /// <param name="toEmailCsv"></param>
       /// <param name="fromEmail"></param>
       /// <param name="subject"></param>
       /// <param name="plainTextMessage"></param>
       /// <param name="htmlMessage"></param>
       /// <param name="replyToEmail"></param>
       /// <param name="importance"></param>
       /// <param name="isTransactional">only used in some implementations, use false for marketing or bulk email</param>
       /// <param name="fromName"></param>
       /// <param name="replyToName"></param>
       /// <param name="toAliasCsv"></param>
       /// <param name="ccEmailCsv"></param>
       /// <param name="ccAliasCsv"></param>
       /// <param name="bccEmailCsv"></param>
       /// <param name="bccAliasCsv"></param>
       /// <param name="attachments"></param>
       /// <param name="charsetBodyHtml">only used in some implementations</param>
       /// <param name="charsetBodyText">only used in some implementations</param>
       /// <param name="configLookupKey">when using the ConfigEmailSenderResolver, the lookupkey can be the name of the sender or it can be left out if only one sneder is configured. In cloudscribe Core the siteid is used to lookup the sender for the given site</param>
       /// <returns></returns>
        Task<EmailSendResult> SendEmailAsync(
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
            );

    }
}
