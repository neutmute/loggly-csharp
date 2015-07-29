using System.Collections.Generic;
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
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            var ipLocalEndPoint = new IPEndPoint(ipAddress, 0);
            _udpClient= new UdpClientEx(ipLocalEndPoint);
        }

        public bool IsActive
        {
            get {  return _udpClient.IsActive ; }
        }

        public void Close()
        {
            if (_udpClient.IsActive)
            {
                _udpClient.Close();
            }
        }


        protected override async Task Send(SyslogMessage syslogMessage)
        {
            if (!_udpClient.IsActive)
            {
                var logglyEndpointIp = Dns.GetHostEntry(LogglyConfig.Instance.Transport.EndpointHostname).AddressList[0];
                _udpClient.Connect(logglyEndpointIp, LogglyConfig.Instance.Transport.EndpointPort);
            }

            try
            {
                if (_udpClient.IsActive)
                {
                    var bytes = syslogMessage.GetBytes();
                    await _udpClient.SendAsync(bytes, bytes.Length).ConfigureAwait(false);
                }
                else
                {
                    LogglyException.Throw("Syslog client Socket is not connected.");
                }
            }
            finally
            {
                Close();
            }
        }
    }
}
