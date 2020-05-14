using System;
using System.Collections.Generic;
using System.Linq;
using Loggly.Config;
using Loggly.Transports.Syslog;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Reflection;

namespace Loggly
{

    public class LogglyClient : ILogglyClient
    {
        private IMessageTransport _transport;
        private JsonSerializer JsonSerializer => _jsonSerializer ?? (_jsonSerializer = JsonSerializer.CreateDefault(CreateJsonSerializerSettings()));
        private JsonSerializer _jsonSerializer;
        
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
                    
                    return await _transport.Send(events.Select(BuildMessage)).ConfigureAwait(false);
                }
                else
                {
                    return new LogResponse {Code = ResponseCode.SendDisabled};
                }
            }
            catch (Exception e)
            {
                LogglyException.Throw(e);
                return new LogResponse { Code = ResponseCode.Unknown };
            }
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

        private string ToJson(object value)
        {
            var jsonSerializer = JsonSerializer;
            try
            {
                var sb = new System.Text.StringBuilder(256);
                var sw = new System.IO.StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);

                using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                {
                    lock (jsonSerializer)
                    {
                        jsonWriter.Formatting = jsonSerializer.Formatting;
                        jsonSerializer.Serialize(jsonWriter, value, value.GetType());
                    }
                    return sb.ToString();
                }
            }
            catch
            {
                _jsonSerializer = null; // Reset as it might now be in bad state
                throw;
            }
        }

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            jsonSerializerSettings.Converters.Add(new ToStringConverter(typeof(System.Reflection.Assembly)));
            jsonSerializerSettings.Converters.Add(new ToStringConverter(typeof(System.Reflection.Module)));
            jsonSerializerSettings.Converters.Add(new ToStringConverter(typeof(System.Reflection.MethodInfo)));
            jsonSerializerSettings.Error = (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing exception property '{args.ErrorContext.Member}', property ignored: {args.ErrorContext.Error}");
                args.ErrorContext.Handled = true;
            };
            return jsonSerializerSettings;
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

        public class ToStringConverter : JsonConverter
        {
            private readonly Type _type;

            /// <inheritdoc />
            public override bool CanRead { get; } = false;

            public ToStringConverter(Type type)
            {
                _type = type;
            }

            /// <inheritdoc />
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    writer.WriteValue(value.ToString());
                }
            }

            /// <inheritdoc />
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException("Only serialization is supported");
            }

            /// <inheritdoc />
            public override bool CanConvert(Type objectType)
            {
#if NETSTANDARD1_5
                return _type.GetTypeInfo().IsAssignableFrom(objectType);
#else
                return _type.IsAssignableFrom(objectType);
#endif
            }
        }
    }
}
