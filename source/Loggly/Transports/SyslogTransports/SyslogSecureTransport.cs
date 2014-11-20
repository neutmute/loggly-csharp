using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly.Transports.Syslog
{
    class SyslogSecureTransport : SyslogTransportBase
    {
        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        protected override void Send(SyslogMessage syslogMessage)
        {
            var hostname = LogglyConfig.Instance.Transport.EndpointHostname;
            var client = new TcpClient(hostname, LogglyConfig.Instance.Transport.EndpointPort);
            
            var sslStream = new SslStream(
                client.GetStream(),
                false,
                ValidateServerCertificate,
                null);

            try
            {
                sslStream.AuthenticateAsClient(hostname);
                byte[] messageBytes = syslogMessage.GetBytes();

                sslStream.Write(messageBytes);
                sslStream.Flush();
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
