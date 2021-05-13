using System;
using System.Collections.Generic;
using Loggly.Config;

namespace Loggly
{
    public enum MessageType
    {
        Plain,
        Json
    }
    public class LogglyMessage
    {
        public DateTimeOffset Timestamp { get; set; }
        public SyslogHeader Syslog { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }

        public List<ITag> CustomTags { get; set; }

        public LogglyMessage()
            :this(new List<ITag>(), new SyslogHeader())
        {
        }

        public LogglyMessage(List<ITag> customTags, SyslogHeader syslog)
        {
            CustomTags = customTags;
            Syslog = syslog;
        }

        public override string ToString()
        {
            return string.Format("content={0}", Content);
        }
    }

    public enum HttpRequestType
    {
        Get
        ,Post
    }
}