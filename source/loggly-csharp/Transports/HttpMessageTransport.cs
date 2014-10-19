using System;
using System.Net;
using System.Text;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly
{
    public class HttpMessageTransport : HttpTransportBase, IMessageTransport
    {
        public void Send(LogglyMessage message)
        {
            Send(message, null);
        }
        
        public void Send(LogglyMessage message, Action<Response> callback)
        {
            if (LogglyConfig.Instance.IsValid)
            {
                var request = CreateRequest(message);
                var requestState = new RequestState();

                requestState.Request = request;
                requestState.Payload = message == null ? null : Encoding.UTF8.GetBytes(message.Content);
                requestState.Callback = callback;

                request.BeginGetRequestStream(GetRequestStream, requestState);
            }
            else
            {
                LogglyException.Throw("Loggly configuration is missing or invalid. Did you specify a customer token?");
            }
        }

        private HttpWebRequest CreateRequest(LogglyMessage message)
        {
            var request = CreateRequest(GetSendUrl(), HttpRequestType.Post);

            if (!string.IsNullOrEmpty(LogglyConfig.Instance.Tags.RenderedTagCsv))
            {
                request.Headers.Add("X-LOGGLY-TAG", LogglyConfig.Instance.Tags.RenderedTagCsv);
            }

            switch (message.Type)
            {
                case MessageType.Plain:
                    request.ContentType = "content-type:text/plain";
                    break;
                case MessageType.Json:
                    request.ContentType = "application/json";
                    break;
            }

            return request;
        }

        private string GetSendUrl()
        {
            var url = "https://logs-01.loggly.com/inputs/" + LogglyConfig.Instance.CustomerToken;
            return url;
        }
    }
}