using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    class SyslogTcpTransport : SyslogTransportBase
    {
        protected string Hostname
        {
            get { return LogglyConfig.Instance.Transport.EndpointHostname; }
        }

        protected virtual Task<Stream> GetNetworkStream(TcpClient client)
        {
            return Task.FromResult<Stream>(client.GetStream());
        }

        protected override async Task Send(SyslogMessage syslogMessage)
        {
            var client = new TcpClient();
            client.ConnectAsync(Hostname, LogglyConfig.Instance.Transport.EndpointPort).Wait();

            try
            {
                byte[] messageBytes = syslogMessage.GetBytes();
                var networkStream = await GetNetworkStream(client).ConfigureAwait(false);
                await networkStream.WriteAsync(messageBytes, 0, messageBytes.Length).ConfigureAwait(false);
                await networkStream.FlushAsync().ConfigureAwait(false);
            }
            catch (AuthenticationException e)
            {
                LogglyException.Throw(e, e.Message);
            }
            finally
            {
#if NET_STANDARD
                client.Dispose();
#else
                client.Close();
#endif
            }

        }
    }
}