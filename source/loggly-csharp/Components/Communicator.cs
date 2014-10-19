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

    public interface IMessageTransport
    {
        void Send(LogglyMessage message, Action<Response> callback);
    }

    public interface ISearchTransport
    {
        SearchResponse Search(SearchQuery query);
        EntryJsonResponseBase Search(EventQuery query);

        SearchResponse<T> Search<T>(SearchQuery query);

        FieldResponse Search(FieldQuery query);
    }

    public class SearchTransport : HttpTransportBase, ISearchTransport
    {

        public SearchResponse Search(SearchQuery query)
        {
            var parameters = query.ToParameters();
            return Search<SearchResponse>("apiv2/search", parameters);
        }

        public SearchResponse<T> Search<T>(SearchQuery query)
        {
            var parameters = query.ToParameters();
            return Search<SearchResponse<T>>("apiv2/search", parameters);
        }
        
        public EntryJsonResponseBase Search(EventQuery query)
        {
            var parameters = query.ToParameters();
            return Search<EntryJsonResponseBase>("apiv2/events", parameters);
        }

        public FieldResponse Search(FieldQuery query)
        {

            var parameters = query.ToParameters();
            return Search<FieldResponse>("apiv2/fields", parameters);
        }


        private T Search<T>(string endPoint, IDictionary<string, object> parameters)
            where T : class
        {
            try
            {
                var searchPathAndQuery = BuildPathAndQuery(endPoint, parameters);
                var searchRequest = CreateRequest(searchPathAndQuery, HttpRequestType.Post, null, null);

                using (var response = searchRequest.GetResponse())
                {
                    T responseObject = typeof(T) == typeof(FieldResponse)
                        ? new FieldResponse(JObject.Parse(GetResponseBody(response)), parameters["fieldname"].ToString()) as T
                        : JsonConvert.DeserializeObject<T>(GetResponseBody(response));

                    if (responseObject is SearchResponseBase)
                    {
                        var searchResponse = responseObject as SearchResponseBase;
                        searchResponse.Transport = this;
                    }

                    return responseObject;
                }
            }
            catch (WebException ex)
            {
                LogglyException.Throw(ex, GetResponseBody(ex.Response));
                return null;
            }
            catch (Exception ex)
            {
                LogglyException.Throw(ex, ex.Message);
                return null;
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
    }

    public abstract class HttpTransportBase
    {
        protected HttpWebRequest CreateRequest(string url, HttpRequestType requestType, LogglyMessage message, string headerLogglyTag = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = requestType.ToString().ToUpper();
            request.UserAgent = "loggly-csharp"; //todo: reflect version info
            request.KeepAlive = false;

            if (!string.IsNullOrEmpty(LogglyConfig.Instance.Tags.RenderedTagCsv))
            {
                request.Headers.Add("X-LOGGLY-TAG", LogglyConfig.Instance.Tags.RenderedTagCsv);
            }

            if (LogglyConfig.Instance.Transport.Credentials != null)
            {
                request.Credentials = LogglyConfig.Instance.Transport.Credentials;
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
        protected static void GetRequestStream(IAsyncResult result)
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

        private static ErrorMessage HandleException(Exception exception)
        {
            if (exception is WebException)
            {
                var body = GetResponseBody(((WebException)exception).Response);
                return new ErrorMessage { Error = body, InnerException = exception };
            }
            return new ErrorMessage { Error = "Unknown Error", InnerException = exception };
        }

    }


    public enum HttpRequestType
    {
        Get
        ,Post
    }

    class RequestState : ResponseState
    {
        public byte[] Payload { get; set; }
    }

    class ResponseState
    {
        public HttpWebRequest Request { get; set; }
        public Action<Response> Callback { get; set; }
    }

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