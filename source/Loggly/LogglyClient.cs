using System;
using System.Collections.Generic;
using System.Linq;
using Loggly.Config;
using Loggly.Transports.Syslog;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Loggly
{

    public class LogglyClient : ILogglyClient
    {
        private IMessageTransport _transport;

        internal LogglyClient(IMessageTransport transport)
        {
            _transport = transport;
        }
        public LogglyClient()
        {
            _transport = TransportFactory();
        }
        public async Task<LogResponse> Log(LogglyEvent logglyEvent)
        {
            return await LogWorker(new [] {logglyEvent}).ConfigureAwait(false);
        }

        public async Task<LogResponse> Log(IEnumerable<LogglyEvent> logglyEvents)
        {
            return await LogWorker(logglyEvents.ToArray()).ConfigureAwait(false);
        }

        private async Task<LogResponse> LogWorker(LogglyEvent[] events)
        {
            var response = new LogResponse {Code = ResponseCode.Unknown};
            try
            {
                if (LogglyConfig.Instance.IsEnabled)
                {
                    if (LogglyConfig.Instance.Transport.LogTransport == LogTransport.Https)
                    {
						if (!LogglyConfig.Instance.Transport.IsOmitTimestamp)
						{
							foreach (var e in events)
							{
								// syslog has this data in the header, only need to add it for Http
								e.Data.AddIfAbsent("timestamp", e.Timestamp);
							}
						}
                    }
                    
                    response = await _transport.Send(events.Select(BuildMessage)).ConfigureAwait(false);
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

        protected virtual LogglyMessage BuildMessage(LogglyEvent logglyEvent)
        {
            return new LogglyMessage
                   {
                       Timestamp = logglyEvent.Timestamp,
                       Syslog = logglyEvent.Syslog,
                       Type = MessageType.Json,
                       Content = ToJson(logglyEvent.Data),
                       CustomTags = logglyEvent.Options.Tags
                   };
        }

        private static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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