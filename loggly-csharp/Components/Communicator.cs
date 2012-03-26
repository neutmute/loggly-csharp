using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
   public class Communicator
   {
      public const string POST = "POST";
      public const string GET = "GET";

      private readonly IRequestContext _context;

      public Communicator(IRequestContext context)
      {
         _context = context;
      }

      public void SendPayload(string method, string endPoint, string message, bool json, Action<Response> callback)
      {
         var request = CreateRequest(method, endPoint, false, json);
         var state = new RequestState {Request = request, Payload = message == null ? null : Encoding.UTF8.GetBytes(message), Callback = callback};
         request.BeginGetRequestStream(GetRequestStream, state);
      }

      public T GetPayload<T>(string endPoint, IDictionary<string, object> parameters)
      {
         var pathAndQuery = BuildPathAndQuery(endPoint, parameters);
         var request = CreateRequest(GET, pathAndQuery, true);
         try
         {
            using (var response = request.GetResponse())
            {
               return JsonConvert.DeserializeObject<T>(GetResponseBody(response));
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

      private HttpWebRequest CreateRequest(string method, string endPoint, bool withCredentials)
      {
         return CreateRequest(method, endPoint, withCredentials, false);
      }

      private HttpWebRequest CreateRequest(string method, string endPoint, bool withCredentials, bool json)
      {
         var data = LogglyConfiguration.Data;
         var url = data.ForcedUrl ?? string.Concat(data.Https ? "https://" : "http://", _context.Url);
         var request = (HttpWebRequest) WebRequest.Create(string.Concat(url, endPoint));
         request.Method = method;
         request.Timeout = data.Timeout;
         request.ReadWriteTimeout = data.Timeout;
         request.UserAgent = "loggly-csharp";
         request.KeepAlive = false;
         if (withCredentials) { request.Credentials = data.Credentials; }
         if (json) { request.ContentType = "application/json"; }
         return request;
      }

      private static void GetRequestStream(IAsyncResult result)
      {
         var state = (RequestState) result.AsyncState;
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

      private static void GetResponseStream(IAsyncResult result)
      {
         var state = (ResponseState) result.AsyncState;
         try
         {
            using (var response = (HttpWebResponse) state.Request.EndGetResponse(result))
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
            var body = GetResponseBody(((WebException) exception).Response);
            return new ErrorMessage {Error = body, InnerException = exception};
         }
         return new ErrorMessage {Error = "Unknown Error", InnerException = exception};
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
   }
}