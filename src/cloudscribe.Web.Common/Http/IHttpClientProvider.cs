using System;
using System.Net.Http;

namespace cloudscribe.Web.Common.Http
{
    public interface IHttpClientProvider
    {
        void Dispose();
        HttpClient GetOrCreateHttpClient(Uri baseAddress);
    }
}