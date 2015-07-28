using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
    internal class HttpMessageTransport : HttpTransportBase, IMessageTransport
    {
        private static string _urlSingle;
        private static string _urlBulk;

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
                var httpWebRequest = CreateHttpWebRequest(list);

                using (var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(false))
                {
                    var rawResponse = Response.CreateSuccess(GetResponseBody(response));

                    if (rawResponse.Success)
                    {
                        logResponse = JsonConvert.DeserializeObject<LogResponse>(rawResponse.Raw);
                        logResponse.Code = ResponseCode.Success;
                        
                    }
                    else
                    {
                        logResponse = new LogResponse { Code = ResponseCode.Error, Message = rawResponse.Error.Message };
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

        private HttpWebRequest CreateHttpWebRequest(List<LogglyMessage> message)
        {
            var httpWebRequest = CreateHttpWebRequest(message.Count == 1 ? UrlSingle : UrlBulk, HttpRequestType.Post);

            if (!string.IsNullOrEmpty(RenderedTags))
            {
                httpWebRequest.Headers.Add("X-LOGGLY-TAG", RenderedTags);
            }
            var type = message.First().Type;
            if (!message.TrueForAll(x => type == x.Type))
            {
                LogglyException.Throw("Cannot have mixed Plain and Json messages");
            }

            switch (type)
            {
                case MessageType.Plain:
                    httpWebRequest.ContentType = "content-type:text/plain";
                    break;
                case MessageType.Json:
                    httpWebRequest.ContentType = "application/json";
                    break;
            }
            var builder = new StringBuilder();
            bool isFirst = true;
            foreach (var m in message)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    builder.Append('\n');
                }
                builder.Append(m.Content);
            }
            var contentBytes = Encoding.UTF8.GetBytes(builder.ToString());

            httpWebRequest.ContentLength = contentBytes.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(contentBytes, 0, contentBytes.Length);
                requestStream.Flush();
                requestStream.Close();
            }

            return httpWebRequest;
        }

        protected override string GetRenderedTags()
        {
            var tags = string.Join(",", LogglyConfig.Instance.Tags.GetRenderedTags().ToArray());
            return tags;
        }
    }
}