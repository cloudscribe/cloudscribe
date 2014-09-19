using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;

using cloudscribe.Resources;

namespace cloudscribe.AspNet.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var mailMessage = new MailMessage
            ("me@example.com", message.Destination, message.Subject, message.Body);

            mailMessage.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                //client.
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
