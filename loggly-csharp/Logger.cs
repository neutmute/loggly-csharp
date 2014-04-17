using System;
using System.Collections.Generic;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
   public class Logger : ILogger, IRequestContext
   {
      private string _url = "logs-01.loggly.com/";
      private string _applicationName = "loggly-csharp-app";
      private readonly string _inputKey;
      
      public Logger(string inputKey,
          string applicationName)
      {
          _inputKey = inputKey;
          _applicationName = applicationName;
      }

      public Logger(string inputKey, 
          string alternativeUrl = null,
          string applicationName = null )
      {
          if (!string.IsNullOrEmpty(alternativeUrl))
              _url = alternativeUrl;

          if (!string.IsNullOrEmpty(applicationName))
              _applicationName = applicationName;

         _inputKey = inputKey;
      }

      public LogResponse LogSync(string message)
      {
         return LogSync(message, false);
      }

      public void Log(string message)
      {
         Log(message, false);
      }

      public void Log(string message, Action<LogResponse> callback)
      {
         Log(message, false, callback);
      }

      public string Url
      {
         get { return _url; }
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

      public void Log(string message, string category, IDictionary<string, object> data =null)
      {
          var logEntry = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
                        {
                           {"userAgent","loggly-csharp"}, {"applicationName",_applicationName},
                           {"message", message}, {"category", category}
                        };

          var jsonLogEntry = JsonConvert.SerializeObject(logEntry);
          Log(jsonLogEntry, true);
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
         var exceptionData = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
                             {
                                {"exception", ex.ToString()}
                             };
         Log(message, "error", exceptionData);
      }

      public void Log(string message, bool json)
      {
         Log(message, json, null);
      }

      public void Log(string message, bool json, Action<LogResponse> callback)
      {
         var communicator = new Communicator(this);
         var callbackWrapper = callback == null ? (Action<Response>) null : r =>
         {
            if (r.Success)
            {
               var res = JsonConvert.DeserializeObject<LogResponse>(r.Raw);
               res.Success = true;
               callback(res);
            }
            else
            {
               var res = new LogResponse{ Success = false };
               callback(res);
            }
         };
         communicator.SendPayload(Communicator.POST, string.Concat("inputs/", _inputKey), message, json, callbackWrapper);
      }
   }
}
