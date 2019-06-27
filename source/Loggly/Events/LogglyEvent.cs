using System;

namespace Loggly
{
    public class LogglyEvent
    {
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Metadata to populate into the syslog header
        /// </summary>
        public SyslogHeader Syslog { get; set; }

        public IMessageData Data { get; set; }

        public EventOptions Options { get; set; }

        public LogglyEvent()
            : this(new MessageData())
        {
        }

        public LogglyEvent(MessageData data)
        {
            Options = new EventOptions();
            Syslog = new SyslogHeader();
            Data = data;

            Timestamp = DateTimeOffset.Now;
        }
    }
}