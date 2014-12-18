using System;
using System.Net;
using System.Reflection;
using System.Text;
using Loggly.Config;
using Loggly.Responses;
using Loggly.Transports;

namespace Loggly
{
    internal abstract class HttpTransportBase : TransportBase
    {
        private static readonly string _userAgent;
        static HttpTransportBase ()
        {
            _userAgent = "loggly-csharp " + Assembly.GetAssembly(typeof(HttpMessageTransport)).GetName().Version;
        }
        protected HttpWebRequest CreateHttpWebRequest(string url, HttpRequestType requestType)
        {
            return CreateHttpWebRequest(url, requestType, false);
        }
        protected HttpWebRequest CreateHttpWebRequest(string url, HttpRequestType requestType, bool useProxy)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            
            if (useProxy)
            {
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            request.Method = requestType.ToString().ToUpper();
            request.UserAgent = _userAgent;
            request.KeepAlive = false;
            return request;
        }
        protected static string GetResponseBody(WebResponse response)
        {
            if (response == null)
            {
                return null;
            }
            using (var stream = response.GetResponseStream())
            {
                var sb = new StringBuilder();
                int read;
                do
                {
                    var buffer = new byte[2048];
                    read = stream.Read(buffer, 0, buffer.Length);
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, read));
                } while (read > 0);
                return sb.ToString();
            }
        }
    }
}