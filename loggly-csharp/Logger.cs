using System;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
   public class Logger : ILogger, IRequestContext
   {
      private const string _url = "logs.loggly.com/";
      private readonly string _inputKey;


      public string Url
      {
         get { return _url; }
      }

      public Logger(string inputKey)
      {
         _inputKey = inputKey;
      }

      public LogResponse Log(string message)
      {
         var synchronizer = new AutoResetEvent(false);
         LogResponse response = null;
         LogAsync(message, r =>
         {
            response = r;
            synchronizer.Set();
         });
         synchronizer.WaitOne();
         return response;
      }

      public void LogAsync(string message)
      {
         LogAsync(message, null);
      }

      public void LogAsync(string message, Action<LogResponse> callback)
      {
         var communicator = new Communicator(this);
         var callbackWrapper = callback == null ? (Action<Response>)null : r =>
         {
            if (r.Success)
            {
               callback(JsonConvert.DeserializeObject<LogResponse>(r.Raw));
            }
         };
         communicator.SendPayload(Communicator.POST, string.Concat("inputs/", _inputKey), message, callbackWrapper);
      }      
   }
}