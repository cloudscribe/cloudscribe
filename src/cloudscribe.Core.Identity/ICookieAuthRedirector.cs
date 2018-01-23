using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace cloudscribe.Core.Identity
{
    public interface ICookieAuthRedirector
    {
        Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector);
    }
}