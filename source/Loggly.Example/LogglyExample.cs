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

        public void SendPlainMessage()
        {
            var logEvent = new LogglyEvent();
            logEvent.Data.Add("message", "Simple message at {0} using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);
            _loggly.Log(logEvent);
        }

        public void SendWithCallback()
        {

            var logEvent = new LogglyEvent();

            logEvent.Options.Callback = lr => Debug.WriteLine(lr);
            logEvent.Data.Add("message", "Simple message at {0} with callback using {1}", DateTime.Now, LogglyConfig.Instance.Transport.LogTransport);

            _loggly.Log(logEvent);
        }

        public void SendWithAttributes()
        {
            var logEvent = new LogglyEvent();

            logEvent.Options.Callback = lr => Debug.WriteLine(lr);
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
