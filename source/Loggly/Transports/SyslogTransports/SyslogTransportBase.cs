using System;
using System.Collections.Generic;
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

            var renderedTags = GetRenderedTags(message.CustomTags);
            var tagSpacer = string.IsNullOrEmpty(renderedTags) ? string.Empty : " ";

            syslogMessage.Text = string.Format(
                                    "[{0}@41058{1}{2}] {3}"
                                    , LogglyConfig.Instance.CustomerToken
                                    , tagSpacer
                                    , renderedTags
                                    , message.Content);

            return syslogMessage;
        }

        protected abstract void Send(SyslogMessage syslogMessage);

        protected override string GetRenderedTags(List<ITag> customTags)
        {
            var sb = new StringBuilder();
            var tagList = new List<ITag>();

            tagList.AddRange(LogglyConfig.Instance.TagConfig.Tags);
            tagList.AddRange(customTags);

            foreach (var tag in tagList.ToLegalStrings())
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