using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net;

namespace Loggly.Config
{
    public interface ITagConfiguration
    {
        List<ISimpleTag> SimpleTags { get;  }

        List<ComplexTag> ComplexTags { get;  }

        string RenderedTagCsv { get; }
    }

    public interface IHttpTransport
    {
    }

    public interface ITransportConfiguration
    {
        ICredentials Credentials {get;set;}
    }

    public interface ISimpleTag
    {
        string Value { get; }
    }

    public class SimpleTag : ISimpleTag
    {
        public string Value { get; set; }
    }

    public class TagConfiguration : ITagConfiguration
    {
        private string _renderedTagCsv;

        public List<ISimpleTag> SimpleTags { get; private set; }

        public List<ComplexTag> ComplexTags { get; private set; }

        public string RenderedTagCsv {
            get
            {
                if (_renderedTagCsv == null)
                {
                    _renderedTagCsv = string.Join(",", GetRenderedTags().ToArray());
                }
                return _renderedTagCsv;
            }
        }

        public TagConfiguration()
        {
            SimpleTags = new List<ISimpleTag>();
            ComplexTags = new List<ComplexTag>();
            _renderedTagCsv = null;
        }


        public List<string> GetRenderedTags()
        {
            var renderedTags = new List<string>();
            SimpleTags.ForEach(st => renderedTags.Add(st.Value));
            ComplexTags.ForEach(ct => renderedTags.Add(ct.FormattedValue));
            return renderedTags;
        }
    }

    public class TransportConfiguration : ITransportConfiguration
    {
        //public IHttpTransport Http { get; private set; }

        public ICredentials Credentials { get; set; }

    }

    public interface ILogglyConfig
    {
        string CustomerToken { get; set; }

        bool ThrowExceptions { get; set; }

        bool IsValid { get;  }

        ITagConfiguration Tags { get;  }
        ITransportConfiguration Transport { get;  }
    }

    public class LogglyConfig : ILogglyConfig
    {
        public string CustomerToken { get; set; }
        public bool ThrowExceptions { get; set; }
        public ITagConfiguration Tags { get; private set; }
        public ITransportConfiguration Transport { get; private set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(CustomerToken); }
        }

        private LogglyConfig()
        {
            Tags = new TagConfiguration();
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
            foreach (ISimpleTag simpleTag in LogglyAppConfig.Instance.Tags.Simple)
            {
                config.Tags.SimpleTags.Add(simpleTag);
            }
            foreach (ComplexTagAppConfig complexTagConfig in LogglyAppConfig.Instance.Tags.Complex)
            {
                var complexTag = (ComplexTag) Activator.CreateInstance(complexTagConfig.Assembly, complexTagConfig.Type).Unwrap();
                config.Tags.ComplexTags.Add(complexTag);
            }

            var username = LogglyAppConfig.Instance.Transport.Credentials.Username;
            var password = LogglyAppConfig.Instance.Transport.Credentials.Password;
            var domain = LogglyAppConfig.Instance.Transport.Credentials.Domain;
    
            config.Transport.Credentials = new NetworkCredential(username, password, domain);
            return config;
        }
    }
}
