using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;

using cloudscribe.Resources;

namespace cloudscribe.AspNet.Identity
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
