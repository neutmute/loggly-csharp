using System;
using System.Linq;
using System.Net;
using System.Text;
using Loggly.Config;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
    internal class HttpMessageTransport : HttpTransportBase, IMessageTransport
    {
        private static string __url;

        private static string Url
        {
            get
            {
                if (string.IsNullOrEmpty(__url))
                {
                    __url = string.Format("https://{0}:{1}/inputs/{2}"
                        , LogglyConfig.Instance.Transport.EndpointHostname
                        , LogglyConfig.Instance.Transport.EndpointPort
                        , LogglyConfig.Instance.CustomerToken);
                }
                return __url;
            }
        }

        public LogResponse Send(LogglyMessage message)
        {
            var logResponse = new LogResponse();

            if (LogglyConfig.Instance.IsValid)
            {
                var httpWebRequest = CreateHttpWebRequest(message);

                using (var response = httpWebRequest.GetResponse())
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
            }
            else
            {
                LogglyException.Throw("Loggly configuration is missing or invalid. Did you specify a customer token?");
            }
            LogglyEventSource.Instance.Log(message, logResponse);
            return logResponse;
        }

        private HttpWebRequest CreateHttpWebRequest(LogglyMessage message)
        {
            var httpWebRequest = CreateHttpWebRequest(Url, HttpRequestType.Post);

            if (!string.IsNullOrEmpty(RenderedTags))
            {
                httpWebRequest.Headers.Add("X-LOGGLY-TAG", RenderedTags);
            }

            switch (message.Type)
            {
                case MessageType.Plain:
                    httpWebRequest.ContentType = "content-type:text/plain";
                    break;
                case MessageType.Json:
                    httpWebRequest.ContentType = "application/json";
                    break;
            }

            var contentBytes = Encoding.UTF8.GetBytes(message.Content);

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