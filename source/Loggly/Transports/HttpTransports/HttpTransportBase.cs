using System.Net.Http;
using System.Reflection;
using Loggly.Transports;

namespace Loggly
{
    internal abstract class HttpTransportBase : TransportBase
    {
        private static readonly string _userAgent;
        static HttpTransportBase ()
        {
            _userAgent = "loggly-csharp " + typeof(HttpMessageTransport).GetTypeInfo().Assembly.GetName().Version;
        }
        protected HttpClient CreateHttpClient(HttpRequestType requestType)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
            httpClient.DefaultRequestHeaders.Add("Connection", "close");
            HttpMethod method = null;
            switch (requestType)
            {
                case HttpRequestType.Get:
                    method = HttpMethod.Get;
                    break;
                case HttpRequestType.Post:
                    method = HttpMethod.Post;
                    break;
            }

            return httpClient;
        }

        protected string GetResponseBody(HttpResponseMessage response)
        {
            if (response == null)
            {
                return null;
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}