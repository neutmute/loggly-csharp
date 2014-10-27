using System.Net;
using System.Net.Sockets;

namespace Loggly.Transports.Syslog
{
    /// <summary>
    /// Exposes the Active propery of UdpClient
    /// </summary>
    public class UdpClientEx : UdpClient
    {
        public UdpClientEx() : base() { }
        public UdpClientEx(IPEndPoint ipe) : base (ipe) { }
        ~UdpClientEx()
        {
            if (this.Active) this.Close();
        }

        public bool IsActive
        {
            get {  return this.Active ; }
        }
    }
}