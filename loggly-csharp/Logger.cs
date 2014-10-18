using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
   public class Logger : ILogger, IRequestContext
   {
      private readonly string _url = "logs-01.loggly.com/";
      private string _applicationName = "loggly-csharp-app";
      private readonly string _inputKey;
      
      public Logger(string inputKey, string applicationName) 
          : this(inputKey,null,applicationName){}
      
      public Logger(string inputKey, string alternativeUrl = null, string applicationName = null)
      {
          if (!string.IsNullOrEmpty(alternativeUrl))
          {
              _url = alternativeUrl;
          }

          if (!string.IsNullOrEmpty(applicationName))
          {
              _applicationName = applicationName;
          }

          _inputKey = inputKey;
      }

      public string Url
      {
         get { return _url; }
      }


      //public void Log(string message, IDictionary<string, object> data = null)
      //{
      //    var logEntry = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
      //                  {
      //                     {"userAgent","loggly-csharp"}, {"applicationName",_applicationName},
      //                     {"message", message},
      //                  };

      //    var jsonLogEntry = JsonConvert.SerializeObject(logEntry);
      //    Log(jsonLogEntry);
      //}
       
      public void Log(string message)
      {
         Log(message,  null);
      }

      public void Log(string message, Action<LogResponse> callback)
      {
          var communicator = new Communicator(this);
          var callbackWrapper = callback == null ? (Action<Response>)null : r =>
          {
              if (r.Success)
              {
                  var res = JsonConvert.DeserializeObject<LogResponse>(r.Raw);
                  res.Success = true;
                  callback(res);
              }
              else
              {
                  var res = new LogResponse { Success = false };
                  callback(res);
              }
          };
          communicator.SendPayload(Communicator.POST, "inputs/", _inputKey, true, callbackWrapper);
      }


      public void LogJson<TMessage>(TMessage message)
      {
          LogJson(message, null);
      }

      public void LogJson<TMessage>(TMessage message, Action<LogResponse> callback)
      {
          var enumerableMessage = message as IEnumerable;
          if (enumerableMessage != null)
          {
              var sb = new StringBuilder();
              foreach (object messageLine in enumerableMessage)
              {
                  sb.AppendLine(ToString(messageLine));
              }

              Log(sb.ToString(),  callback);
          }
          else
          {
              Log(ToString(message), callback);
          }
      }

      private static string ToString(object messageLine)
      {
          if (messageLine is string)
          {
              return messageLine as string;
          }

          var asJson = JsonConvert.SerializeObject(messageLine, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          return asJson;
      }

      public void LogJson(string message)
      {
          Log(message);
      }

      public void LogJson(string message, Action<LogResponse> callback)
      {
          Log(message, callback);
      }


   }
}
