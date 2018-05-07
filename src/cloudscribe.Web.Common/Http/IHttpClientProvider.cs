using System;
using System.Net.Http;

namespace cloudscribe.Web.Common.Http
{
    public interface IHttpClientProvider : IDisposable
    {
        HttpClient GetOrCreateHttpClient(Uri baseAddress);
    }
}