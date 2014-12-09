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
            var sysLog = ConstructSyslog(message);
            Send(sysLog);

            var response = new LogResponse { Code = ResponseCode.AssumedSuccess };

            LogglyEventSource.Instance.Log(message, response);

            return response;
        }

        internal SyslogMessage ConstructSyslog(LogglyMessage message)
        {
            var appNameSafe = LogglyConfig.Instance.ApplicationName ?? string.Empty;
            
            var syslogMessage = new SyslogMessage();

            syslogMessage.Facility = Facility.User;
            syslogMessage.Text = message.Content;
            syslogMessage.Level = message.Syslog.Level;
            syslogMessage.MessageId = message.Syslog.MessageId;
            syslogMessage.AppName = appNameSafe.Replace(" ", "");
            syslogMessage.Timestamp = message.Timestamp;

            var tags = RenderedTags;
            var tagSpacer = string.IsNullOrEmpty(RenderedTags) ? string.Empty : " ";

            syslogMessage.Text = string.Format(
                                    "[{0}@41058{1}{2}] {3}"
                                    , LogglyConfig.Instance.CustomerToken
                                    , tagSpacer
                                    , tags
                                    , message.Content);

            return syslogMessage;
        }

        protected abstract void Send(SyslogMessage syslogMessage);

        protected override string GetRenderedTags()
        {
            var sb = new StringBuilder();
            var tagList = LogglyConfig.Instance.Tags.GetRenderedTags();
            foreach (var tag in tagList)
            {
                sb.AppendFormat("tag=\"{0}\" ", tag);
            }
            if (tagList.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}