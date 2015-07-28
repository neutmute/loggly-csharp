using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Loggly.Responses;

namespace Loggly.Transports.Syslog
{
    class SyslogSecureTransport : SyslogTcpTransport
    {
        private static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        protected override async Task<Stream> GetNetworkStream(TcpClient client)
        {
            var sslStream = new SslStream(
                client.GetStream(),
                false,
                ValidateServerCertificate,
                null
                );

            await sslStream.AuthenticateAsClientAsync(Hostname).ConfigureAwait(false);

            return sslStream;
        }
    }
}
