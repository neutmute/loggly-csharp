using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Loggly.Config;
using Loggly.Responses;
using Loggly.Transports;
using Loggly.Transports.Syslog;
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
            IMessageTransport transporter = GetTransport();
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
                        var res = new LogResponse {Success = false, Message = r.Error.Message};
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

            IMessageTransport transporter = GetTransport();
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

        private IMessageTransport GetTransport()
        {
            switch (LogglyConfig.Instance.MessageTransport)
            {
                case MessageTransport.Http:         return new HttpMessageTransport();
                case MessageTransport.SyslogUdp:    return new SyslogUdpTransport();
                case MessageTransport.SyslogSecure: return new SyslogSecureTransport();
                default: throw new NotSupportedException("Unsupported transport: " + LogglyConfig.Instance.MessageTransport);
            }
        }
    }
}