using System;
using System.Net;
using Loggly.Responses;
using Loggly.Transports.Syslog;

namespace Loggly
{
    internal enum MessageType
    {
        Plain,
        Json
    }
    internal class LogglyMessage
    {
        public DateTimeOffset Timestamp { get; set; }
        public SyslogHeader Syslog { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }

        public LogglyMessage()
        {
            Syslog = new SyslogHeader();
        }
    }

    internal enum HttpRequestType
    {
        Get
        ,Post
    }

    //class RequestState : ResponseState
    //{
    //    public byte[] Payload { get; set; }
    //}

    //class ResponseState
    //{
    //    public HttpWebRequest Request { get; set; }
    //    //public Action<Response> Callback { get; set; }
    //}
}