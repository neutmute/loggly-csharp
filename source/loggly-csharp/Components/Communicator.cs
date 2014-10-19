using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Loggly.Config;
using Loggly.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loggly
{
    public enum MessageType
    {
        Plain,
        Json
    }
    public class LogglyMessage
    {
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }

    public interface ISendTransport
    {
        void Send(LogglyMessage message);
    }

    public class HttpTransporter : ISendTransport
    {
        public const string POST = "POST";
        public const string GET = "GET";


        public void SendPayload(string method, string endPoint, string message, bool json, Action<Response> callback)
        {
            var request = CreateRequest(method, endPoint, false, json, new string[0]);
            var state = new RequestState { Request = request, Payload = message == null ? null : Encoding.UTF8.GetBytes(message), Callback = callback };
            request.BeginGetRequestStream(GetRequestStream, state);
        }

        public T GetPayload<T>(string endPoint, IDictionary<string, object> parameters)
            where T : class
        {
            try
            {
                var searchPathAndQuery = BuildPathAndQuery(endPoint, parameters);
                var searchRequest = CreateRequest(GET, searchPathAndQuery, true, null);

                using (var response = searchRequest.GetResponse())
                {
                    T responseObject = typeof(T) == typeof(FieldResponse)
                        ? new FieldResponse(JObject.Parse(GetResponseBody(response)), parameters["fieldname"].ToString()) as T
                        : JsonConvert.DeserializeObject<T>(GetResponseBody(response));
                    if (responseObject is SearchResponseBase)
                    {
                        var searchResponse = responseObject as SearchResponseBase;
                        searchResponse.Communicator = this;
                    }

                    return responseObject;
                }
            }
            catch (WebException ex)
            {
                throw new LogglyException(GetResponseBody(ex.Response), ex);
            }
            catch (Exception ex)
            {
                throw new LogglyException(ex.Message, ex);
            }
        }

        private static string BuildPathAndQuery(string endPoint, ICollection<KeyValuePair<string, object>> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return endPoint;
            }
            var sb = new StringBuilder(100);
            sb.Append(endPoint);
            sb.Append('?');
            foreach (var kvp in parameters)
            {
                if (kvp.Value == null)
                {
                    continue;
                }
                sb.Append(kvp.Key);
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(kvp.Value.ToString()));
                sb.Append("&");
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        private HttpWebRequest CreateRequest(string method, string endPoint, bool withCredentials, string[] tags)
        {
            return CreateRequest(method, endPoint, withCredentials, false, tags);
        }

        private HttpWebRequest CreateRequest(string method, string endPoint, bool withCredentials, bool json, string[] tags)
        {
            var data = LogglyConfiguration.Data;

            var request = (HttpWebRequest)WebRequest.Create(GetSendUrl());
            request.Method = POST;
            request.Timeout = data.Timeout;
            request.ReadWriteTimeout = data.Timeout;
            request.UserAgent = "loggly-csharp"; //todo: reflect version info
            request.KeepAlive = false;
            if (tags != null && tags.Length > 0)
                request.Headers.Add("X-LOGGLY-TAG", string.Join(",", tags));
            if (withCredentials) { request.Credentials = data.Credentials; }
            if (json) { request.ContentType = "application/json"; }
            return request;
        }

        private string GetSendUrl()
        {
            var url = "https://logs-01.loggly.com/inputs/" + LogglyConfig.Instance.CustomerToken;
            return url;
        }

        private static void GetRequestStream(IAsyncResult result)
        {
            var state = (RequestState)result.AsyncState;
            try
            {
                if (state.Payload != null)
                {
                    using (var requestStream = state.Request.EndGetRequestStream(result))
                    {
                        requestStream.Write(state.Payload, 0, state.Payload.Length);
                        requestStream.Flush();
                        requestStream.Close();
                    }
                }
                state.Request.BeginGetResponse(GetResponseStream, state);
            }
            catch (Exception ex)
            {
                if (state.Callback != null)
                {
                    state.Callback(Response.CreateError(HandleException(ex)));
                }
            }
        }

        private static void GetResponseStream(IAsyncResult result)
        {
            var state = (ResponseState)result.AsyncState;
            try
            {
                using (var response = (HttpWebResponse)state.Request.EndGetResponse(result))
                {
                    if (state.Callback != null)
                    {
                        state.Callback(Response.CreateSuccess(GetResponseBody(response)));
                    }
                }
            }
            catch (Exception ex)
            {
                if (state.Callback != null)
                {
                    state.Callback(Response.CreateError(HandleException(ex)));
                }
            }
        }

        private static string GetResponseBody(WebResponse response)
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

        private static ErrorMessage HandleException(Exception exception)
        {
            if (exception is WebException)
            {
                var body = GetResponseBody(((WebException)exception).Response);
                return new ErrorMessage { Error = body, InnerException = exception };
            }
            return new ErrorMessage { Error = "Unknown Error", InnerException = exception };
        }

        private class RequestState : ResponseState
        {
            public byte[] Payload { get; set; }
        }

        private class ResponseState
        {
            public HttpWebRequest Request { get; set; }
            public Action<Response> Callback { get; set; }
        }

        public void Send(LogglyMessage message)
        {
            throw new NotImplementedException();
        }
    }
}