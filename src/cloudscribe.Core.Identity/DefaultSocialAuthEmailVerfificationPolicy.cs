using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Core.Identity
{
    public class DefaultSocialAuthEmailVerfificationPolicy : ISocialAuthEmailVerfificationPolicy
    {
        public DefaultSocialAuthEmailVerfificationPolicy(
           // ILogger<DefaultSocialAuthEmailVerfificationPolicy> logger
            )
        {
           // log = logger;
        }

        //private ILogger log;

        public bool HasVerifiedEmail(ExternalLoginInfo externalLoginInfo)
        {
            //if (externalLoginInfo.Principal == null) return false;
            //https://github.com/pennersr/django-allauth/issues/145
            // TODO: for now returning false so confirmation is still required
            // but can revisit this later
            // we know that google auth provides a profile property email_verified
            //but can we access it
            //var emailVerified = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.);
            // not sure about Microsoft
            // twitter doesn't provide an email
            //if(externalLoginInfo.Principal != null)
            //{
            //    foreach(var c in externalLoginInfo.Principal.Claims)
            //    {
            //        var message = $"found claim type {c.Type} with value {c.Value}";
            //        log.LogWarning(message);
            //    }

                
            //}

            return false;
        }
    }
}
