using System;
using System.Net;
using System.Text;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly
{
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
            var exceptionAsWebException = exception as WebException;
            if (exceptionAsWebException != null)
            {
                var body = GetResponseBody(exceptionAsWebException.Response);
                return new ErrorMessage { Message = body, InnerException = exception };
            }
            return new ErrorMessage { Message = "Unknown Error", InnerException = exception };
        }

    }
}