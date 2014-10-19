using System;
using System.Net;
using System.Text;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly
{
    public class HttpTransporter : HttpTransportBase, IMessageTransport
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
            return CreateRequest(GetSendUrl(), HttpRequestType.Get, message, LogglyConfig.Instance.Tags.RenderedTagCsv);
        }

        private string GetSendUrl()
        {
            var url = "https://logs-01.loggly.com/inputs/" + LogglyConfig.Instance.CustomerToken;
            return url;
        }
    }
}