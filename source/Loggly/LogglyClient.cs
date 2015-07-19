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
            return Task.Run(() => LogWorker(logglyEvent));
        }

        private LogResponse LogWorker(LogglyEvent logglyEvent)
        {
            var response = new LogResponse {Code = ResponseCode.Unknown};
            try
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
                        , Type = MessageType.Json
                        , Content = ToJson(logglyEvent.Data)
                    };
                
                    IMessageTransport transporter = TransportFactory();
                    response = transporter.Send(message);
                }
                else
                {
                    response = new LogResponse {Code = ResponseCode.SendDisabled};
                }
            }
            catch (Exception e)
            {
                LogglyException.Throw(e);
            }
            return response;
        }

        private static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
            });
        }
        
        private IMessageTransport TransportFactory()
        {
            var transport = LogglyConfig.Instance.Transport.LogTransport;
            switch (transport)
            {
                case LogTransport.Https: return new HttpMessageTransport();
                case LogTransport.SyslogUdp: return new SyslogUdpTransport();
                case LogTransport.SyslogTcp: return new SyslogTcpTransport();
                case LogTransport.SyslogSecure: return new SyslogSecureTransport();
                default: throw new NotSupportedException("Unsupported transport: " + transport);
            }
        }
    }
}