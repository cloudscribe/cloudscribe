using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace app.IntegrationTests.Helpers
{
    public class TestHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (WrappedMessageHandler == null) throw new Exception("You must set WrappedMessageHandler before TestHttpMessageHandler can be used.");
            var method = typeof(HttpMessageHandler).GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = method.Invoke(this.WrappedMessageHandler, new object[] { request, cancellationToken });
            return (Task<HttpResponseMessage>)result;
        }

        public HttpMessageHandler WrappedMessageHandler { get; set; }
    }
}
