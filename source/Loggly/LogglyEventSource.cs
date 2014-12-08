using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggly
{
    /// <remarks>
    /// this.Guid	{36623ac9-597b-5ef6-5158-0410d4d5dc73}
    /// 
    /// logman -ets create trace logglyTrace -p {36623ac9-597b-5ef6-5158-0410d4d5dc73} -o D:\Logs\logglyTrace.etl
    /// logman –ets stop logglyTrace
    /// </remarks>
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

        public void Log(LogglyMessage message, LogResponse result)
        {
            WriteEvent(_counterNonThreadSafe++, message.ToString(), result.ToString());
        }
    }
}
