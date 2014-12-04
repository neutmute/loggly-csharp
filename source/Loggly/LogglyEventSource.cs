using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggly
{
    sealed class LogglyEventSource : EventSource
    {
        #region Singleton
        private static int _counterNonThreadSafe;
        private static readonly Lazy<LogglyEventSource> _instance = new Lazy<LogglyEventSource>(() => new LogglyEventSource());

        public static LogglyEventSource Instance
        {
            get { return _instance.Value; }
        }

        private LogglyEventSource()
        {
            _counterNonThreadSafe = 0;
        }

        #endregion

        //public void Log(string messageFormat, params object[] messageArgs)
        //{
        //    WriteEvent(1, string.Format(messageFormat, messageArgs));
        //}

        public void Log(LogglyMessage message, LogResponse result)
        {
            WriteEvent(_counterNonThreadSafe++, message.ToString(), result.ToString());
        }
    }
}
