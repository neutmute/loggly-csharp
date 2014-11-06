using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.Events
{
    public class MessageDataFixture : Fixture
    {
        /// <summary>
        /// Fix for this exception..
        /// 
        /// NLog.NLogRuntimeException NLog.NLogRuntimeException: Exception occurred in NLog ---> System.FormatException: Index (zero based) must be greater than or equal to zero and less than the size of the argument list.
        /// at System.Text.StringBuilder.AppendFormat(IFormatProvider provider, String format, Object[] args)
        /// at System.String.Format(IFormatProvider provider, String format, Object[] args)
        /// at Loggly.MessageData.Add(String key, String valueFormat, Object[] valueArgs)
        /// at NLog.Targets.Loggly.Write(LogEventInfo logEvent)
        /// at NLog.Targets.Target.Write(AsyncLogEventInfo logEvent)
        /// </summary>
        [Test]
        public void AddDoesntThrowWhenLooksLikeFormat()
        {
            var logglyEvent = new LogglyEvent();
            const string logMessage  = "a message that looks like it should have arg params {0}";
            logglyEvent.Data.Add("message", logMessage);
        }
    }
}
