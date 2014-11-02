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
using System.Threading.Tasks;

namespace Loggly
{

    public class LogglyClient : ILogglyClient
    {
        public Task<LogResponse> Log(LogglyEvent logglyEvent)
        {
            var task = new Task<LogResponse>(() => LogWorker(logglyEvent));

            //task.ContinueWith<LogResponse>(t => 
            //{ throw LogglyException.Throw(t.Exception as Exception, "loggly task Exception"); }, logglyEvent, TaskContinuationOptions.OnlyOnFaulted);
            task.Start();
            return task;
        }

        private LogResponse LogWorker(LogglyEvent logglyEvent)
        {
            if (LogglyConfig.Instance.IsEnabled)
            {
                if (LogglyConfig.Instance.Transport.LogTransport == LogTransport.Https)
                {
                    // syslog has this data in the header, only need to add it for Http
                    logglyEvent.Data.AddIfAbsent("timestamp", logglyEvent.Timestamp);
                }

                var message = new LogglyMessage
                {
                    Timestamp = logglyEvent.Timestamp
                    , Syslog = logglyEvent.Syslog
                    , Type = MessageType.Plain
                    , Content = ToJson(logglyEvent.Data)
                };
                
                IMessageTransport transporter = TransportFactory();
                return transporter.Send(message);
            }
            else
            {
                return new LogResponse {Code = ResponseCode.SendDisabled};
            }
        }

        private static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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