using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace cloudscribe.Email.Senders
{
    public class DefaultServiceClientProvider : IServiceClientProvider   // Register as singleton
    {
        public DefaultServiceClientProvider()
        {
            _httpClients = new ConcurrentDictionary<Uri, HttpClient>();
        }

        private readonly ConcurrentDictionary<Uri, HttpClient> _httpClients;

        public HttpClient GetOrCreateHttpClient(Uri baseAddress)
        {
            return _httpClients.GetOrAdd(baseAddress,
                b => new HttpClient { BaseAddress = b });
        }

        public void Dispose()
        {
            foreach (var httpClient in _httpClients.Values)
            {
                httpClient.Dispose();
            }
        }

    }
}
