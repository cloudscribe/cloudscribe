//using System;
//using System.Collections.Concurrent;
//using System.Net.Http;

//namespace cloudscribe.Web.Common.Http
//{
//    public class DefaultHttpClientProvider : IHttpClientProvider //register as singleton
//    {
//        public DefaultHttpClientProvider()
//        {
//            _httpClients = new ConcurrentDictionary<Uri, HttpClient>();
//        }

//        private readonly ConcurrentDictionary<Uri, HttpClient> _httpClients;

//        public HttpClient GetOrCreateHttpClient(Uri baseAddress)
//        {
//            return _httpClients.GetOrAdd(baseAddress,
//                b => new HttpClient { BaseAddress = b });
//        }

//        public void Dispose()
//        {
//            foreach (var httpClient in _httpClients.Values)
//            {
//                httpClient.Dispose();
//            }
//        }
//    }
//}
