using System;
using System.Diagnostics;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{

    #region Level enum

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

    #endregion

    #region Facility enum

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

    #endregion

    public class SyslogMessage
    {
        #region Properties

        public DateTimeOffset Timestamp { get; set; }
        public int MessageId { get; set; }
        public Facility Facility { get; set; }

        public Level Level { get; set; }

        public string Text { get; set; }

        public string AppName { get; set; }

        public IEnvironmentProvider EnvironmentProvider { get; set; }

        #endregion
        
        #region Ctor

        public SyslogMessage()
        {
            EnvironmentProvider = new EnvironmentProvider();
        }

        public SyslogMessage(Facility facility, Level level, string text) : this()
        {
            Facility = facility;
            Level = level;
            Text = text;
        }

        #endregion

        #region Methods

        internal string GetMessageAsString()
        {
            int priority = (((int) Facility)*8) + ((int) Level);

            var msg = String.Format(
                "<{0}>1 {1} {2} {3} {4} {5} {6}\n"
                , priority
                , Timestamp.ToSyslog()
                , EnvironmentProvider.MachineName
                , AppName
                , EnvironmentProvider.ProcessId
                , MessageId
                , Text);

            return msg;
        }

        public byte[] GetBytes()
        {
            var messageString = GetMessageAsString();
            byte[] bytes = Encoding.ASCII.GetBytes(messageString);
            return bytes;
        }

        #endregion
    }
}