using Loggly.Transports.Syslog;

namespace Loggly
{
    public class SyslogHeader
    {
        public int MessageId { get; set; }

        public Level Level { get; set; }

        public SyslogHeader()
        {
            Level = Level.Information;
        }
    }
}