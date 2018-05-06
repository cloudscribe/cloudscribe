using System;
using System.Net.Http;

namespace cloudscribe.Email.Senders
{
    public interface IServiceClientProvider
    {
        void Dispose();
        HttpClient GetOrCreateHttpClient(Uri baseAddress);
    }
}