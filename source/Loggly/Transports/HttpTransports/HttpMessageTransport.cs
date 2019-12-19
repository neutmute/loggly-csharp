using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using Loggly.Responses;
using Loggly.Transports;
using Newtonsoft.Json;

namespace Loggly
{
    internal class HttpMessageTransport : TransportBase, IMessageTransport
    {
        private static string _urlSingle;
        private static string _urlBulk;

        private static readonly string _userAgent;
        protected HttpClient HttpClient;

        static HttpMessageTransport()
        {
            _userAgent = "loggly-csharp " + typeof(HttpMessageTransport).GetTypeInfo().Assembly.GetName().Version;
        }

        internal HttpMessageTransport(HttpMessageHandler messageHandler)
        {
            HttpClient = new HttpClient(messageHandler);
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
            HttpClient.DefaultRequestHeaders.Add("Connection", "close");

            if (!string.IsNullOrWhiteSpace(LogglyConfig.Instance.Transport.ForwardedForIp))
            {
                HttpClient.DefaultRequestHeaders.Add("X-Forwarded-For", LogglyConfig.Instance.Transport.ForwardedForIp);
            }
        }

        public HttpMessageTransport() : this(new HttpClientHandler())
        {

        }


        private static string UrlSingle
        {
            get
            {
                if (string.IsNullOrEmpty(_urlSingle))
                {
                    _urlSingle = string.Format("https://{0}:{1}/inputs/{2}"
                        , LogglyConfig.Instance.Transport.EndpointHostname
                        , LogglyConfig.Instance.Transport.EndpointPort
                        , LogglyConfig.Instance.CustomerToken);
                }
                return _urlSingle;
            }
        }

        private static string UrlBulk
        {
            get
            {
                if (string.IsNullOrEmpty(_urlBulk))
                {
                    _urlBulk = string.Format("https://{0}:{1}/bulk/{2}"
                        , LogglyConfig.Instance.Transport.EndpointHostname
                        , LogglyConfig.Instance.Transport.EndpointPort
                        , LogglyConfig.Instance.CustomerToken);
                }
                return _urlBulk;
            }
        }

        public async Task<LogResponse> Send(IEnumerable<LogglyMessage> messages)
        {
            var logResponse = new LogResponse();

            if (LogglyConfig.Instance.IsValid)
            {
                var list = messages.ToList();

                using (var response = await PostUsingHttpClient(list).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        logResponse = new LogResponse { Code = ResponseCode.Success};
                    }
                    else
                    {
                        logResponse = new LogResponse { Code = ResponseCode.Error, Message = "Loggly returned status:" + response.StatusCode };
                    }
                }
                foreach (var m in list)
                {
                    LogglyEventSource.Instance.Log(m, logResponse);
                }

            }
            else
            {
                LogglyException.Throw("Loggly configuration is missing or invalid. Did you specify a customer token?");
            }
            return logResponse;
        }

        private Task<HttpResponseMessage> PostUsingHttpClient(List<LogglyMessage> messages)
        {
            string tags;
            if (messages.Count == 1)
            {
                tags = GetRenderedTags(messages[0].CustomTags);
            }
            else
            {
                // if bulk sending messages, send all tags which have the same value for all messages
                tags = string.Join(",", messages.SelectMany(x => GetLegalTagUnion(x.CustomTags)).GroupBy(x => x).Where(x => x.Count() == messages.Count).Select(x => x.Key));
            }

            var request=new HttpRequestMessage(HttpMethod.Post, messages.Count == 1 ? UrlSingle : UrlBulk);
            
            if (!string.IsNullOrEmpty(tags))
            {
                request.Headers.Add("X-LOGGLY-TAG", tags);
            }
            var type = messages.First().Type;
            if (!messages.TrueForAll(x => type == x.Type))
            {
                LogglyException.Throw("Cannot have mixed Plain and Json messages");
            }

            string messageContent;
            if (messages.Count == 1)
            {
                messageContent = messages[0].Content;
            }
            else
            {
                var builder = new StringBuilder(messages[0].Content);
                for (var i = 1; i < messages.Count; i++)
                {
                    builder.Append('\n');
                    builder.Append(messages[i].Content);
                }
                messageContent = builder.ToString();
            }

            StringContent postData = null;

            switch (type)
            {
                case MessageType.Plain:
                    postData = new StringContent(messageContent, Encoding.UTF8, "text/plain");
                    break;
                case MessageType.Json:
                    postData = new StringContent(messageContent, Encoding.UTF8, "application/json");
                    break;
            }

            request.Content = postData;
            return HttpClient.SendAsync(request);
        }

        protected override string GetRenderedTags(List<ITag> customTags)
        {
            var tags = string.Join(",", GetLegalTagUnion(customTags));
            return tags;
        }

        private async Task<string> GetResponseBodyAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}