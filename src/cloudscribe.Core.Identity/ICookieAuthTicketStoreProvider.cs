using Microsoft.AspNetCore.Authentication.Cookies;

namespace cloudscribe.Core.Identity
{
    public interface ICookieAuthTicketStoreProvider
    {
        ITicketStore GetTicketStore();
    }

    public class NoopCookieAuthTicketStoreProvider : ICookieAuthTicketStoreProvider
    {
        public ITicketStore GetTicketStore()
        {
            return null;
        }

    }
}
