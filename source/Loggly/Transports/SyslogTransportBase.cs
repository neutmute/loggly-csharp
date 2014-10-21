using System;
using System.Text;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    internal abstract class SyslogTransportBase : TransportBase, IMessageTransport
    {
        public void Send(LogglyMessage message, Action<Responses.Response> callback)
        {
            var syslogMessage = new SyslogMessage();
            syslogMessage.Facility = Facility.User;
            syslogMessage.Text = message.Content;
            syslogMessage.Level = message.Level;
            syslogMessage.MessageId = message.MessageId;
            Send(syslogMessage);
        }

        protected abstract void Send(SyslogMessage syslogMessage);

        protected override string GetRenderedTags()
        {
            var sb = new StringBuilder();
            foreach (var tag in LogglyConfig.Instance.Tags.GetRenderedTags())
            {
                sb.AppendFormat("tag=\"{0}\" ", tag);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}