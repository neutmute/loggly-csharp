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
    /*
     * $DefaultNetstreamDriverCAFile /etc/rsyslog.d/keys/ca.d/loggly_full.crt
$ActionSendStreamDriver gtls
$ActionSendStreamDriverMode 1
$ActionSendStreamDriverAuthMode x509/name
$ActionSendStreamDriverPermittedPeer *.loggly.com

*.* @@logs-01.loggly.com:6514;LogglyFormat
     */
    class SyslogSecureTransport : SyslogTransportBase
    {
        // The following method is invoked by the RemoteCertificateValidationDelegate. 
        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers. 
            return false;
        }

        protected override void Send(SyslogMessage syslogMessage)
        {
            const string endpoint = "logs-01.loggly.com";
            var client = new TcpClient(endpoint, 6514);
            
            // Create an SSL stream that will close the client's stream.
            var sslStream = new SslStream(
                client.GetStream(),
                false,
                ValidateServerCertificate,
                null
                );

            // The server name must match the name on the server certificate. 
            try
            {
                sslStream.AuthenticateAsClient(endpoint);
                byte[] messageBytes = syslogMessage.GetBytes();

                sslStream.Write(messageBytes);
                sslStream.Flush();

               // var serverMessage = ReadMessage(sslStream);
               // Console.WriteLine("Server says: {0}", serverMessage);
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


        private static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the server. 
            // The end of the message is signaled using the 
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8 
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF. 
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

    }

    //public class SslTcpClient
    //{
    //    private static Hashtable certificateErrors = new Hashtable();

        
    //    public static void RunClient(string machineName, string serverName)
    //    {
            
    //        Console.WriteLine("Client closed.");
    //    }
    //    private static void DisplayUsage()
    //    {
    //        Console.WriteLine("To start the client specify:");
    //        Console.WriteLine("clientSync machineName [serverName]");
    //        Environment.Exit(1);
    //    }
    //    public static int Main(string[] args)
    //    {
    //        string serverCertificateName = null;
    //        string machineName = null;
    //        if (args == null || args.Length < 1)
    //        {
    //            DisplayUsage();
    //        }
    //        // User can specify the machine name and server name. 
    //        // Server name must match the name on the server's certificate. 
    //        machineName = args[0];
    //        if (args.Length < 2)
    //        {
    //            serverCertificateName = machineName;
    //        }
    //        else
    //        {
    //            serverCertificateName = args[1];
    //        }
    //        SslTcpClient.RunClient(machineName, serverCertificateName);
    //        return 0;
    //    }
    //}
}
