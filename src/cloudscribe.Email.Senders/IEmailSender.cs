using System.Threading.Tasks;

//TODO: comments and documentation

namespace cloudscribe.Email
{
    public interface IEmailSender
    {
        string Name { get; }

        Task<bool> IsConfigured(string configLookupKey = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEmailCsv"></param>
        /// <param name="fromEmail"></param>
        /// <param name="subject"></param>
        /// <param name="plainTextMessage"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="replyToEmail"></param>
        /// <param name="importance"></param>
        /// <param name="isTransactional"></param>
        /// <param name="fromName"></param>
        /// <param name="replyToName"></param>
        /// <param name="toAliasCsv"></param>
        /// <param name="ccEmailCsv"></param>
        /// <param name="ccAliasCsv"></param>
        /// <param name="bccEmailCsv"></param>
        /// <param name="bccAliasCsv"></param>
        /// <param name="attachmentFilePaths"></param>
        /// <param name="charsetBodyHtml"></param>
        /// <param name="charsetBodyText"></param>
        /// <param name="configLookupKey"></param>
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
            string[] attachmentFilePaths = null,
            string charsetBodyHtml = null,
            string charsetBodyText = null,
            string configLookupKey = null
            );

    }
}
