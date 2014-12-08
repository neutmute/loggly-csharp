using System;
using System.Text;
using Loggly.Config;
using Loggly.Responses;

namespace Loggly.Transports.Syslog
{
    internal abstract class SyslogTransportBase : TransportBase, IMessageTransport
    {
        public LogResponse Send(LogglyMessage message)
        {
            var appNameSafe = LogglyConfig.Instance.ApplicationName ?? string.Empty;


            var syslogMessage = new SyslogMessage();

            syslogMessage.Facility = Facility.User;
            syslogMessage.Text = message.Content;
            syslogMessage.Level = message.Syslog.Level;
            syslogMessage.MessageId = message.Syslog.MessageId;
            syslogMessage.AppName = appNameSafe.Replace(" ", "");
            syslogMessage.Timestamp = message.Timestamp;

            syslogMessage.Text = string.Format(
                                    "[{0} {1}] {2}"
                                    , LogglyConfig.Instance.CustomerToken
                                    , RenderedTags
                                    , message.Content);
            
            Send(syslogMessage);

            var response = new LogResponse { Code = ResponseCode.AssumedSuccess };

            LogglyEventSource.Instance.Log(message, response);

            return response;
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