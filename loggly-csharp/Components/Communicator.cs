using System;
using System.Net;
using System.Text;
using Loggly.Responses;

namespace Loggly
{
   public class Communicator
   {
      public const string POST = "POST";      

      private readonly IRequestContext _context;

      public Communicator(IRequestContext context)
      {
         _context = context;
      }

      public void SendPayload(string method, string endPoint, string message, Action<Response> callback)
      {
         var data = LogglyConfiguration.Data;
         var url = data.ForcedUrl ?? string.Concat(data.Https ? "https://" : "http://", _context.Url);         
         var request = (HttpWebRequest)WebRequest.Create(string.Concat(url, endPoint));
         request.Method = method;                 
         request.Timeout = data.Timeout;
         request.UserAgent = "loggly-csharp";
         request.ReadWriteTimeout = data.Timeout;
         request.KeepAlive = false;
         request.BeginGetRequestStream(GetRequestStream, new RequestState { Request = request, Payload = Encoding.UTF8.GetBytes(message), Callback = callback });
      }

      private void GetRequestStream(IAsyncResult result)
      {
         var state = (RequestState)result.AsyncState;
         using (var requestStream = state.Request.EndGetRequestStream(result))
         {
            requestStream.Write(state.Payload, 0, state.Payload.Length);
            requestStream.Flush();
            requestStream.Close();
         }
         state.Request.BeginGetResponse(GetResponseStream, state);         
      }

      private void GetResponseStream(IAsyncResult result)
      {
         var state = (ResponseState)result.AsyncState;
         try
         {
            var response = (HttpWebResponse)state.Request.EndGetResponse(result);
            if (state.Callback != null)
            {
               state.Callback(Response.CreateSuccess(GetResponseBody(response)));
            }
         }
         catch (Exception ex)
         {
            throw;
            if (state.Callback != null)
            {
               state.Callback(Response.CreateError(HandleException(ex)));
            }
         }
      }


 
      private static string GetResponseBody(WebResponse response)
      {
         using (var stream = response.GetResponseStream())
         {
            var buffer = new byte[response.ContentLength == -1 ? 1024 : response.ContentLength];
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
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


      private class ResponseState
      {
         public HttpWebRequest Request { get; set; }
         public Action<Response> Callback { get; set; }
      }
      private class RequestState : ResponseState
      {
         public byte[] Payload { get; set; }
      }
   }
}