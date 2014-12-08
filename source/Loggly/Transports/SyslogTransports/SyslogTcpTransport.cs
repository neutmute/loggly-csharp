using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    class SyslogTcpTransport : SyslogTransportBase
    {
        protected string Hostname
        {
            get { return LogglyConfig.Instance.Transport.EndpointHostname; }
        }

        protected virtual Stream GetNetworkStream(TcpClient client)
        {
            return client.GetStream();
        }

        protected override void Send(SyslogMessage syslogMessage)
        {
            var client = new TcpClient(Hostname, LogglyConfig.Instance.Transport.EndpointPort);

            try
            {
                byte[] messageBytes = syslogMessage.GetBytes();
                var networkStream = GetNetworkStream(client);
                networkStream.Write(messageBytes, 0, messageBytes.Length);
                networkStream.Flush();
            }
            catch (AuthenticationException e)
            {
                LogglyException.Throw(e, e.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}