using System;
using System.Collections.Generic;
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
        UUCP=8, 
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
        private int _facility; 
        public int Facility
        {
            get { return _facility;}
            set { _facility=value; }
        }
        private int _level; 
        public int Level
        {
            get { return _level;}
            set { _level=value; }
        }
        private string _text; 
        public string Text
        {
            get { return _text;}
            set { _text=value; }
        }
        public SyslogMessage() {}
        public SyslogMessage (int facility, int level, string text)
        {
            _facility= facility;
            _level= level;
            _text= text;
        }
    }


    /// need this helper class to expose the Active propery of UdpClient
    /// (why is it protected, anyway?) 
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


    public class SyslogMessageTransport : IMessageTransport
    {
        public void Send(LogglyMessage message, Action<Responses.Response> callback)
        {
            var syslogMessage = new SyslogMessage();
            syslogMessage.Text = message.Content;
            syslogMessage.Facility = 1;
            syslogMessage.Level = (int) Level.Information;
        }

        
        private IPHostEntry _ipHostInfo;
        private IPAddress _ipAddress;
        private IPEndPoint      _ipLocalEndPoint;
        private UdpClientEx _udpClient;
        private int _port= 514;

        public SyslogMessageTransport()
        {
            _ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddress = _ipHostInfo.AddressList[0];
            _ipLocalEndPoint = new IPEndPoint(_ipAddress, 0);
            _udpClient= new UdpClientEx(_ipLocalEndPoint);
        }

        public bool IsActive
        {
            get {  return _udpClient.IsActive ; }
        }

        public void Close()
        {
            if (_udpClient.IsActive) _udpClient.Close();
        }

        public int Port
        {
            set {_port= value; }
            get {return _port; } 
        }

        public void Send(SyslogMessage syslogMessage)
        {
            if (!_udpClient.IsActive)
            {
                var logglyEndpointIp = Dns.GetHostEntry("logs-01.loggly.com").AddressList[0];
                _udpClient.Connect(logglyEndpointIp, _port);
            }

            if (_udpClient.IsActive)
            {
                int priority = syslogMessage.Facility*8 + syslogMessage.Level;
                string msg = String.Format(
                    "<{0}>1 {1} {2} {3} {4} {5} [{6}] {7}\n"
                    ,priority
                    ,DateTime.Now.ToLogglyDateTime()
                    ,Environment.MachineName
                    ,"yourAppName"
                    ,"1" // processId
                    ,"2" // messageId
                    ,LogglyConfig.Instance.CustomerToken
                    ,syslogMessage.Text
                    );
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                _udpClient.Send(bytes, bytes.Length);
            }
            else
            {
                throw new Exception ("Syslog client Socket is not connected. Please set the SysLogServerIp property");
            }
        }
    }
}
