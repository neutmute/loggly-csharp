using System;
using System.Net;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    internal class SyslogUdpTransport : SyslogTransportBase
    {
        private readonly UdpClientEx _udpClient;

        public SyslogUdpTransport()
        {
            var localEP = new IPEndPoint(IPAddress.Any, 0);
            _udpClient = new UdpClientEx(localEP);
        }

        public bool IsActive
        {
            get { return _udpClient.IsActive; }
        }

        public void Close()
        {
            if (_udpClient.IsActive)
            {
#if NETSTANDARD
                _udpClient.Dispose();
#else
                _udpClient.Close();
#endif
            }
        }

        protected override async Task<LogResponse> Send(SyslogMessage syslogMessage)
        {
            try
            {
                var bytes = syslogMessage.GetBytes();
                await _udpClient.SendAsync(
                    bytes,
                    bytes.Length,
                    LogglyConfig.Instance.Transport.EndpointHostname,
                    LogglyConfig.Instance.Transport.EndpointPort).ConfigureAwait(false);
                return new LogResponse() { Code = ResponseCode.Success };
            }
            catch (Exception ex)
            {
                LogglyException.Throw(ex, "Error when sending data using Udp client.");
                return new LogResponse() { Code = ResponseCode.Error, Message = $"{ex.GetType()}: {ex.Message}" };
            }
            finally
            {
                Close();
            }
        }
    }
}
