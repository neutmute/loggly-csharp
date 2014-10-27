using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loggly.Config
{
    internal partial class TransportAppConfig : ITransportConfiguration
    {

        public static ITransportConfiguration ConformToValidConfig(ITransportConfiguration input)
        {
            var newConfig = new TransportConfiguration();
            if (input != null)
            {
                newConfig.EndpointHostname = input.EndpointHostname;
                newConfig.EndpointPort = input.EndpointPort;
                newConfig.LogTransport = input.LogTransport;
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
