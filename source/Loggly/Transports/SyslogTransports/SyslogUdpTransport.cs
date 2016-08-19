using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    internal class SyslogUdpTransport : SyslogTransportBase
    {
        private readonly UdpClientEx _udpClient;
        public SyslogUdpTransport()
        {
            var ipHostInfo = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
            var ipAddress = ipHostInfo.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            var ipLocalEndPoint = new IPEndPoint(ipAddress, 0);
            _udpClient = new UdpClientEx(ipLocalEndPoint);
        }

        public bool IsActive
        {
            get { return _udpClient.IsActive; }
        }

        public void Close()
        {
            if (_udpClient.IsActive)
            {
#if NET_STANDARD
                _udpClient.Dispose();
#else
                _udpClient.Close();
#endif
            }
        }


        protected override async Task Send(SyslogMessage syslogMessage)
        {
            try
            {
                var hostEntry = Dns.GetHostEntryAsync(LogglyConfig.Instance.Transport.EndpointHostname).Result;
                var logglyEndpointIp = hostEntry.AddressList[0];
                var bytes = syslogMessage.GetBytes();
                await _udpClient.SendAsync(
                    bytes,
                    bytes.Length,
                    new IPEndPoint(logglyEndpointIp, LogglyConfig.Instance.Transport.EndpointPort)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogglyException.Throw(ex, "Error when sending data using Udp client.");
            }
            finally
            {
                Close();
            }
        }
    }
}
