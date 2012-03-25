using System;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Loggly
{
   public class Logger : ILogger, IRequestContext
   {
      private const string _url = "logs.loggly.com/";
      private readonly string _inputKey;


      public Logger(string inputKey)
      {
         _inputKey = inputKey;
      }

      public LogResponse LogSync(string message)
      {
          return this.LogSync(message, false);
      }

      public LogResponse LogSync(string message, bool json)
      {
         var synchronizer = new AutoResetEvent(false);
         
         LogResponse response = null;
         Log(message, json, r =>
         {
            response = r;
            synchronizer.Set();
         });

         synchronizer.WaitOne();
         return response;
      }

      public void Log(string message, string category)
      {
          Log(message, category, null);
      }

      public void Log(string message, string category, IDictionary<string, object> data)
      {
          var logEntry = new Dictionary<string, object>(data != null ? data : new Dictionary<string, object>());
          logEntry.Add("message", message);
          logEntry.Add("category", category);

          string jsonLogEntry = JsonConvert.SerializeObject(logEntry);

          this.Log(jsonLogEntry, true);
      }

      public void LogInfo(string message)
      {
          LogInfo(message, null);
      }
      
       public void LogInfo(string message, IDictionary<string, object> data)
      {
          Log(message, "info", data);
      }

       public void LogVerbose(string message)
       {
           LogVerbose(message, null);
       }

       public void LogVerbose(string message, IDictionary<string, object> data)
       {
           Log(message, "verbose", data);
       }

       public void LogWarning(string message)
       {
           LogWarning(message, null);
       }

       public void LogWarning(string message, IDictionary<string, object> data)
       {
           Log(message, "warning", data);
       }

      public void LogError(string message, Exception ex)
      {
          LogError(message, ex, null);
      }

      public void LogError(string message, Exception ex, IDictionary<string, object> data)
      {
          var exceptionData = new Dictionary<string, object>(data != null ? data : new Dictionary<string, object>());
          exceptionData.Add("exception", ex.ToString());

          Log(message, "error", exceptionData);
      }

      public void Log(string message)
      {
          Log(message, false);
      }

      public void Log(string message, bool json)
      {
          Log(message, json, null);
      }
      
       public void Log(string message, Action<LogResponse> callback)
      {
          this.Log(message, false, callback);
      }

      public void Log(string message, bool json, Action<LogResponse> callback)
      {
         var communicator = new Communicator(this);
         var callbackWrapper = callback == null ? (Action<Response>) null : r =>
         {
            if (r.Success)
            {
               callback(JsonConvert.DeserializeObject<LogResponse>(r.Raw));
            }
         };

         communicator.SendPayload(Communicator.POST, string.Concat("inputs/", _inputKey), message, json, callbackWrapper);
      }

      public string Url
      {
         get { return _url; }
      }
   }
}