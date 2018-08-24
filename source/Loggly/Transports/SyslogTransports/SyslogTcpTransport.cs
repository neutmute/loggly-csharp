using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    class SyslogTcpTransport : SyslogTransportBase, IDisposable
    {
        private Stream _networkStream;
        private TcpClient _tcpClient;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

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
            await _semaphore.WaitAsync();
            
            try
            {
                if (_networkStream == null)
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(Hostname, LogglyConfig.Instance.Transport.EndpointPort).ConfigureAwait(false);
                    _networkStream = await GetNetworkStream(_tcpClient).ConfigureAwait(false);
                }

                byte[] messageBytes = syslogMessage.GetBytes();
                
                await _networkStream.WriteAsync(messageBytes, 0, messageBytes.Length).ConfigureAwait(false);
                await _networkStream.FlushAsync().ConfigureAwait(false);
            }
            catch (AuthenticationException e)
            {
                LogglyException.Throw(e, e.Message);
            }
            catch (IOException ioException)
            {
#if NET_STANDARD
                _tcpClient?.Dispose();
#else
                _tcpClient?.Close();
#endif
                _networkStream = null;
                LogglyException.Throw(ioException, ioException.Message);
            }
            catch (ObjectDisposedException disposedException)
            {
                _networkStream = null;
                LogglyException.Throw(disposedException, disposedException.Message);
            }
            finally
            {
                _semaphore.Release();
            }

        }

        public void Dispose()
        {
#if NET_STANDARD
            _tcpClient?.Dispose();
#else
            _tcpClient?.Close();
#endif
            _semaphore?.Dispose();
        }
    }
}