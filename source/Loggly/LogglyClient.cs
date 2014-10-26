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
        public void Log(LogglyEvent logglyEvent)
        {
            if (LogglyConfig.Instance.IsEnabled)
            { 
                logglyEvent.Data.AddSafe("timestamp", logglyEvent.Timestamp);
                var message = new LogglyMessage { Syslog= logglyEvent.Syslog, Type = MessageType.Plain, Content = ToJson(logglyEvent.Data) };
                var callbackWrapper = GetCallbackWrapper(logglyEvent.Options.Callback);

                IMessageTransport transporter = TransportFactory();
                transporter.Send(message, callbackWrapper);
            }
        }

        private static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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