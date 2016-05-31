using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config
{
    public static class ITransportConfigurationExtensions
    {
        /// <summary>
        /// If host/port aren't specified sets to loggly defaults based on selected transport
        /// </summary>
        public static ITransportConfiguration GetCoercedToValidConfig(this ITransportConfiguration input)
        {
            var newConfig = new TransportConfiguration();
            if (input != null)
            {
                newConfig.EndpointHostname = input.EndpointHostname;
                newConfig.EndpointPort = input.EndpointPort;
                newConfig.LogTransport = input.LogTransport;
				newConfig.IsOmitTimestamp = input.IsOmitTimestamp;
            }

            if (string.IsNullOrEmpty(newConfig.EndpointHostname))
            {
                newConfig.EndpointHostname = "logs-01.loggly.com";
            }

            if (newConfig.EndpointPort == 0)
            {
                switch (newConfig.LogTransport)
                {
                    case LogTransport.Https:
                        newConfig.EndpointPort = 443;
                        break;
                    case LogTransport.SyslogUdp:
                    case LogTransport.SyslogTcp:
                        newConfig.EndpointPort = 514;
                        break;
                    case LogTransport.SyslogSecure:
                        newConfig.EndpointPort = 6514;
                        break;
                }
            }

            return newConfig;
        }
    }
}
