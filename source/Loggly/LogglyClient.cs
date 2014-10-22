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
    public class MessageOptions
    {
        /// <summary>
        /// Only valid for Http transport
        /// </summary>
        public Action<LogResponse> Callback { get; set; }

        /// <summary>
        /// Only valid for syslog transport
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Only valid for syslog transport
        /// </summary>
        public Level Level { get; set; }

        public MessageOptions()
        {
            Level = Level.Information;
        }
    }


    public class LogglyClient : ILogglyClient
    {
        public void Log(string plainTextFormat, params object[] plainTextArgs)
        {
            Log(new MessageOptions(), plainTextFormat, plainTextArgs);
        }
        public void Log(MessageOptions options, string plainTextFormat, params object[] plainTextArgs)
        {
            IMessageTransport transporter = TransportFactory();
            var callbackWrapper = GetCallbackWrapper(options.Callback);
            var messageText = string.Format(plainTextFormat, plainTextArgs);
            var message = new LogglyMessage {MessageId= options.MessageId, Type = MessageType.Plain, Content = messageText};

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
            Log(null, logObject);
        }

        public void Log<TMessage>(MessageOptions options, TMessage logObject)
        {
            if (options == null)
            {
                options = new MessageOptions();
            }
            var message = new LogglyMessage { MessageId = options.MessageId, Level=options.Level, Type = MessageType.Plain, Content = ToJson(logObject) };
            var callbackWrapper = GetCallbackWrapper(options.Callback);

            IMessageTransport transporter = TransportFactory();
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

        private IMessageTransport TransportFactory()
        {
            var transport = LogglyConfig.Instance.Transport.LogTransport;
            switch (transport)
            {
                case LogTransport.Https: return new HttpMessageTransport();
                case LogTransport.SyslogUdp: return new SyslogUdpTransport();
                case LogTransport.SyslogSecure: return new SyslogSecureTransport();
                default: throw new NotSupportedException("Unsupported transport: " + transport);
            }
        }
    }
}