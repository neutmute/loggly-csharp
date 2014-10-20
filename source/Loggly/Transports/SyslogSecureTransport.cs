using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            const string endpoint = "logs-01.loggly.com";
            var client = new TcpClient(endpoint, 6514);
            
            var sslStream = new SslStream(
                client.GetStream(),
                false,
                ValidateServerCertificate,
                null
                );

            try
            {
                sslStream.AuthenticateAsClient(endpoint);
                byte[] messageBytes = syslogMessage.GetBytes(RenderedTags);

                sslStream.Write(messageBytes);
                sslStream.Flush();
            }
            catch (AuthenticationException e)
            {
                LogglyException.Throw(e, e.Message);
                return;
            }
            finally
            {
                client.Close();
            }
        }
    }
}
