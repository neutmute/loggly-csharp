using System;

namespace Loggly.Transports.Syslog
{
    internal abstract class SyslogTransportBase : IMessageTransport
    {
        public void Send(LogglyMessage message, Action<Responses.Response> callback)
        {
            var syslogMessage = new SyslogMessage();
            syslogMessage.Text = message.Content;
            syslogMessage.Facility = Facility.User;
            syslogMessage.Level = Level.Information;
            Send(syslogMessage);
        }

        protected abstract void Send(SyslogMessage syslogMessage);
    }
}