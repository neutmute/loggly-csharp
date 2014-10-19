using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Loggly.Responses;
using Newtonsoft.Json;

namespace Loggly
{
    public class LogglyClient : ILogglyClient
    {
        public void Log(string plainTextFormat, params object[] plainTextArgs)
        {
            Log(null, plainTextFormat, plainTextArgs);
        }

        public void Log(Action<LogResponse> callback, string plainTextFormat, params object[] plainTextArgs)
        {
            IMessageTransport transporter = new HttpTransporter();
            var callbackWrapper = GetCallbackWrapper(callback);
            var messageText = string.Format(plainTextFormat, plainTextArgs);
            var message = new LogglyMessage { Type = MessageType.Plain, Content = messageText};

            transporter.Send(message, callbackWrapper);
        }

        private static Action<Response> GetCallbackWrapper(Action<LogResponse> callback)
        {
            Action<Response> callbackWrapper = null;

            if (callback != null)
            {
                callbackWrapper = r =>
                {
                    if (r.Success)
                    {
                        var res = JsonConvert.DeserializeObject<LogResponse>(r.Raw);
                        res.Success = true;
                        callback(res);
                    }
                    else
                    {
                        var res = new LogResponse {Success = false};
                        callback(res);
                    }
                };
            }

            return callbackWrapper;
        }

        public void Log<TMessage>(TMessage logObject)
        {
            Log(logObject, null);
        }

        public void Log<TMessage>(TMessage logObject, Action<LogResponse> callback)
        {
            var message = new LogglyMessage { Type = MessageType.Plain, Content = ToJson(logObject) };
            var callbackWrapper = GetCallbackWrapper(callback);

            IMessageTransport transporter = new HttpTransporter();
            transporter.Send(message, callbackWrapper);
        }

        private static string ToJson(object objectToLog)
        {
            var objectToLogAsString = objectToLog as string;

            if (objectToLogAsString != null)
            {
                return objectToLogAsString;
            }

            var asJson = JsonConvert.SerializeObject(objectToLog, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            return asJson;
        }
    }
}