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
            LogResponse logResponse = null;

            foreach (var message in messages)
            {
                var sysLog = ConstructSyslog(message);
                var response = await Send(sysLog).ConfigureAwait(false);
                if (logResponse == null || logResponse.Code == ResponseCode.Success)
                    logResponse = response;
                LogglyEventSource.Instance.Log(message, logResponse);
            }

            return logResponse ?? new LogResponse { Code = ResponseCode.AssumedSuccess };
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

        protected abstract Task<LogResponse> Send(SyslogMessage syslogMessage);

        private string GetRenderedTags(List<ITag> customTags)
        {
            var tags = GetLegalTagUnion(customTags);
            if (tags.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
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