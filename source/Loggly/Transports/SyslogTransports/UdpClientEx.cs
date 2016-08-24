using System.Net;
using System.Net.Sockets;

namespace Loggly.Transports.Syslog
{
    /// <summary>
    /// Exposes the Active propery of UdpClient
    /// </summary>
    public class UdpClientEx : UdpClient
    {
        public UdpClientEx() { }
        public UdpClientEx(IPEndPoint ipe) : base (ipe) { }
        ~UdpClientEx()
        {
            if (Active)
#if NET_STANDARD
                Dispose();
#else
                Close();
#endif
        }

        public bool IsActive
        {
            get {  return Active ; }
        }
    }
}