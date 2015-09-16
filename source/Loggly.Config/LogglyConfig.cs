using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net;

namespace Loggly.Config
{
    public class LogglyConfig : ILogglyConfig
    {
        public string ApplicationName { get; set; }
        public string CustomerToken { get; set; }
        public bool ThrowExceptions { get; set; }
        public ITagConfiguration TagConfig { get; private set; }
        public ITransportConfiguration Transport { get; set; }
        public ISearchConfiguration Search { get; private set; }

        public bool IsEnabled { get;set;}
        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(CustomerToken); }
        }

        private LogglyConfig()
        {
            IsEnabled = true;
            TagConfig = new TagConfiguration();
            Transport = new TransportConfiguration();
        }

        private static ILogglyConfig _instance;

        public static ILogglyConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (LogglyAppConfig.HasAppCopnfig)
                    {
                        _instance = FromAppConfig();
                    }
                    else
                    {
                        _instance = GetNullConfig();
                    }
                }
                return _instance;
            }
            set { _instance = value; }
        }

        private static ILogglyConfig GetNullConfig()
        {
            return new LogglyConfig();
        }

        private static ILogglyConfig FromAppConfig()
        {
            var config = new LogglyConfig();

            config.CustomerToken = LogglyAppConfig.Instance.CustomerToken;
            config.ThrowExceptions = LogglyAppConfig.Instance.ThrowExceptions;
            config.ApplicationName = new ApplicationNameProvider().GetName();
            config.IsEnabled = LogglyAppConfig.Instance.IsEnabled;

            if (LogglyAppConfig.Instance.HasTagConfig)
            {
                config.TagConfig.Tags.AddRange(LogglyAppConfig.Instance.Tags.Simple.Cast<ITag>().ToList());
                config.TagConfig.Tags.AddRange(LogglyAppConfig.Instance.Tags.GetComplexTags());
            }

            config.Transport = LogglyAppConfig.Instance.Transport.GetCoercedToValidConfig();

            config.Search = LogglyAppConfig.Instance.Search;

            return config;
        }
    }
}
