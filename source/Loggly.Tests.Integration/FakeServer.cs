using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Loggly.Tests
{
   public class FakeServer : IDisposable
   {
      public const int Port = 9948;
      private readonly IList<ApiExpectation> _expectations;
      private readonly HttpListener _listener;
      private readonly Thread _thread;
      private bool _disposed;

      public FakeServer()
      {
         _expectations = new List<ApiExpectation>(5);
         _listener = new HttpListener();
         _listener.Prefixes.Add("http://*:" + 9948 + "/");
         _listener.Start();
         _thread = new Thread(Listen) {IsBackground = true};
         _thread.Start();
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      public void Stub(ApiExpectation expectation)
      {
         _expectations.Add(expectation);
      }

      private void Listen()
      {
         while (true)
         {
            var context = GetContext();
            if (context == null)
            {
               return;
            }
            var body = ExtractBody(context.Request);
            var expectation = FindExpectation(context, body);
            if (expectation == null)
            {
               SendResponse(context, string.Format("Unexpected call: {0} {1}{2}{3}", context.Request.HttpMethod, context.Request.Url, Environment.NewLine, body), new ApiExpectation {Status = 500});
               return;
            }
            SendResponse(context, body, expectation);
         }
      }

      private static void SendResponse(HttpListenerContext context, string body, ApiExpectation expectation)
      {
         var response = context.Response;
         response.StatusCode = expectation.Status ?? 200;
         response.ContentLength64 = (expectation.Response ?? body).Length;
         using (var sw = new StreamWriter(response.OutputStream))
         {
            sw.Write(expectation.Response ?? body);
         }
         response.Close();
      }

      private HttpListenerContext GetContext()
      {
         try
         {
            return _listener.GetContext();
         }
         catch (HttpListenerException)
         {
            return null;
         }
      }

      private ApiExpectation FindExpectation(HttpListenerContext context, string body)
      {
         var request = context.Request;
         foreach (var expectation in _expectations)
         {
            if (expectation.Method != null && string.Compare(request.HttpMethod, expectation.Method, true) != 0)
            {
               continue;
            }
            if (expectation.Url != null && string.Compare(request.Url.AbsolutePath, expectation.Url, true) != 0)
            {
               continue;
            }
            if (expectation.Request != null && string.Compare(body, expectation.Request, true) != 0)
            {
               continue;
            }
            if (expectation.QueryString != null && string.Compare(request.Url.Query, expectation.QueryString, true) != 0)
            {
               continue;
            }
            return expectation; //we found a match!
         }
         return null;
      }

      private static string ExtractBody(HttpListenerRequest request)
      {
         var buffer = new byte[request.ContentLength64];
         request.InputStream.Read(buffer, 0, buffer.Length);
         return Encoding.Default.GetString(buffer);
      }


      private void Dispose(bool disposing)
      {
         if (!_disposed && disposing)
         {
            _listener.Stop();
         }
         _disposed = true;
      }
   }

   /// <remarks>
   /// All of these default to safe values, the most interesting of which is a null response will act as an echo server (return the request)
   /// </remarks>
   public class ApiExpectation
   {
      public static readonly ApiExpectation EchoAll = new ApiExpectation();
      public string Method { get; set; }
      public string Url { get; set; }
      public string QueryString { get; set; }
      public string Request { get; set; }
      public int? Status { get; set; }
      public string Response { get; set; }
   }

   public enum HttpMethod
   {
      POST = 1,
      PUT = 2,
   }
}