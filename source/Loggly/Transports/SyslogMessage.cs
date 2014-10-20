using System;
using System.Diagnostics;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    public enum Level
    {
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Information = 6,
        Debug = 7,
    }

    public enum Facility
    {
        Kernel = 0,
        User = 1,
        Mail = 2,
        Daemon = 3,
        Auth = 4,
        Syslog = 5,
        Lpr = 6,
        News = 7,
        Uucp = 8,
        Cron = 9,
        Local0 = 10,
        Local1 = 11,
        Local2 = 12,
        Local3 = 13,
        Local4 = 14,
        Local5 = 15,
        Local6 = 16,
        Local7 = 17,
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
}