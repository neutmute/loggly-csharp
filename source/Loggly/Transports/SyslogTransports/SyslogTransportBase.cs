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
                await Send(sysLog).ConfigureAwait(false);

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

            var tags = GetRenderedTags(message.CustomTags);
            var tagSpacer = string.IsNullOrEmpty(tags) ? string.Empty : " ";

            syslogMessage.Text = string.Format(
                                    "[{0}@41058{1}{2}] {3}"
                                    , LogglyConfig.Instance.CustomerToken
                                    , tagSpacer
                                    , tags
                                    , message.Content);

            return syslogMessage;
        }

        protected abstract Task Send(SyslogMessage syslogMessage);

        protected override string GetRenderedTags(List<ITag> customTags)
        {
            var sb = new StringBuilder();

            var tags = GetLegalTagUnion(customTags);

            foreach (var tag in tags)
            {
                sb.AppendFormat("tag=\"{0}\" ", tag);
            }
            if (tags.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}