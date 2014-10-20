using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    public enum Level
    {
        Emergency= 0,
        Alert=1, 
        Critical=2, 
        Error=3, 
        Warning=4, 
        Notice=5, 
        Information=6, 
        Debug=7, 
    }

    public enum Facility
    {
        Kernel=0, 
        User=1, 
        Mail=2, 
        Daemon=3, 
        Auth=4, 
        Syslog=5, 
        Lpr=6, 
        News=7, 
        Uucp=8, 
        Cron=9, 
        Local0=10, 
        Local1=11, 
        Local2=12, 
        Local3=13, 
        Local4=14, 
        Local5=15, 
        Local6=16, 
        Local7=17, 
    }


    public class SyslogMessage
    {
        public Facility Facility { get; set; }

        public Level Level { get; set; }

        public string Text { get; set; }

        public SyslogMessage() {}
        public SyslogMessage(Facility facility, Level level, string text)
        {
            Facility= facility;
            Level= level;
            Text= text;
        }

        public byte[] GetBytes()
        {
            int priority = (((int)Facility) * 8) + ((int)Level);
            string msg = String.Format(
                "<{0}>1 {1} {2} {3} {4} {5} [{6}] {7}\n"
                , priority
                , DateTime.Now.ToLogglyDateTime()
                , Environment.MachineName
                , LogglyConfig.Instance.ApplicationName
                , Process.GetCurrentProcess().Id
                , "2" // messageId
                , LogglyConfig.Instance.CustomerToken
                , Text
                );
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            return bytes;
        }
    }


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

    internal abstract class SyslogTransportBase : IMessageTransport
    {
        public void Send(LogglyMessage message, Action<Responses.Response> callback)
        {
            var syslogMessage = new SyslogMessage();
            syslogMessage.Text = message.Content;
            syslogMessage.Facility = Facility.User;
            syslogMessage.Level = Level.Information;
            Send(syslogMessage);
        }

        protected abstract void Send(SyslogMessage syslogMessage);
    }

    internal class SyslogMessageTransport : SyslogTransportBase
    {
        private IPHostEntry _ipHostInfo;
        private IPAddress _ipAddress;
        private IPEndPoint      _ipLocalEndPoint;
        private UdpClientEx _udpClient;
        public int Port { get; set; }

        public SyslogMessageTransport()
        {
            Port = 514;
            _ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddress = _ipHostInfo.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            _ipLocalEndPoint = new IPEndPoint(_ipAddress, 0);
            _udpClient= new UdpClientEx(_ipLocalEndPoint);
        }

        public bool IsActive
        {
            get {  return _udpClient.IsActive ; }
        }

        public void Close()
        {
            if (_udpClient.IsActive)
            {
                _udpClient.Close();
            }
        }


        protected override void Send(SyslogMessage syslogMessage)
        {
            if (!_udpClient.IsActive)
            {
                var logglyEndpointIp = Dns.GetHostEntry("logs-01.loggly.com").AddressList[0];
                _udpClient.Connect(logglyEndpointIp, Port);
            }

            try
            {
                if (_udpClient.IsActive)
                {
                    var bytes = syslogMessage.GetBytes();
                    _udpClient.Send(bytes, bytes.Length);
                }
                else
                {
                    LogglyException.Throw("Syslog client Socket is not connected.");
                }
            }
            finally
            {
                Close();
            }
        }
    }
}
