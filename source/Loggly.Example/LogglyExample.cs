using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Example
{
    class LogglyExample
    {
        readonly ILogglyClient _loggly = new LogglyClient();

        public ResponseCode SendPlainMessageSynchronous()
        {
            var logEvent = new LogglyEvent();
            logEvent.Data.Add("message", "Simple message at {0} using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);
            var r = _loggly.Log(logEvent).Result;
            return r.Code;
        }

        public void SendWithCallback()
        {

            var logEvent = new LogglyEvent();

            logEvent.Data.Add("message", "Simple message at {0} with callback using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);

            _loggly.Log(logEvent);
        }

        public void SendWithAttributes()
        {
            var logEvent = new LogglyEvent();

            logEvent.Data.Add("message", "Message with attributes");
            logEvent.Data.Add("context", new LogObject());

            _loggly.Log(logEvent);
        }

        public void SendCustomObject()
        {
            var logEvent = new LogglyEvent();
            logEvent.Data = new LogObject();
            _loggly.Log(logEvent);
        }
    }
}
