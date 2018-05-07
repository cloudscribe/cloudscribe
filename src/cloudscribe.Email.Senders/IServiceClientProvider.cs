using System;
using System.Net.Http;

namespace cloudscribe.Email.Senders
{
    public interface IServiceClientProvider : IDisposable
    {
        HttpClient GetOrCreateHttpClient(Uri baseAddress);
    }
}