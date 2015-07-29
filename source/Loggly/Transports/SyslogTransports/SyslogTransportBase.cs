using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;

namespace Loggly.Transports.Syslog
{
    internal abstract class SyslogTransportBase : TransportBase, IMessageTransport
    {
        public async Task<LogResponse> Send(IEnumerable<LogglyMessage> messages)
        {
            foreach (var message in messages)
            {
                var sysLog = ConstructSyslog(message);
                await Send(sysLog);

                var response = new LogResponse
                {
                    Code = ResponseCode.AssumedSuccess
                };
                LogglyEventSource.Instance.Log(message, response);
            }

            return new LogResponse
            {
                Code = ResponseCode.AssumedSuccess
            };
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

        protected abstract Task Send(SyslogMessage syslogMessage);

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