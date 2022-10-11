using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class RemainingSessionTimeResolver
    {
        protected IHttpContextAccessor HttpContextAccessor { get; private set; }

        public RemainingSessionTimeResolver(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }


        public virtual async Task<double> RemainingSessionTimeInSeconds()
        {
            double secondsLeft;

            try
            {
                var authResult = await HttpContextAccessor.HttpContext.AuthenticateAsync();
                if (authResult.Succeeded)
                {
                    if (authResult.Properties.ExpiresUtc != null)
                    {
                        secondsLeft = ((DateTimeOffset)authResult.Properties.ExpiresUtc - DateTimeOffset.UtcNow).TotalSeconds;
                    }
                    else
                    {
                        secondsLeft = 0;  // auth success but we haven't managed to read expiry from cookie
                    }
                }
                else { secondsLeft = 0; }
            }
            catch
            {
                secondsLeft = 0;
            }

            // answer in seconds 
            return secondsLeft;
        }
    }
}
