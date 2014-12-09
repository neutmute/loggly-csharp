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

        public override string ToString()
        {
            return string.Format("content={0}", Content);
        }
    }

    internal enum HttpRequestType
    {
        Get
        ,Post
    }
}