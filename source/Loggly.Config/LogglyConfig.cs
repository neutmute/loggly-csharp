using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

    public interface ISearchConfiguration
    {
        string Account { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }

    public class SearchConfiguration : ISearchConfiguration
    {
        public string Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TransportConfiguration : ITransportConfiguration
    {
        //public IHttpTransport Http { get; private set; }
        
    }

    public interface ILogglyConfig
    {
        string CustomerToken { get; set; }
        string ApplicationName { get; set; }

        bool ThrowExceptions { get; set; }

        bool IsValid { get;  }

        ITagConfiguration Tags { get;  }
        ITransportConfiguration Transport { get;  }
        ISearchConfiguration Search { get; }
    }

    public class LogglyConfig : ILogglyConfig
    {
        public string ApplicationName { get; set; }
        public string CustomerToken { get; set; }
        public bool ThrowExceptions { get; set; }
        public ITagConfiguration Tags { get; private set; }
        public ITransportConfiguration Transport { get; private set; }
        public ISearchConfiguration Search { get; private set; }

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
            config.ApplicationName = LogglyAppConfig.Instance.ApplicationName;

            foreach (ISimpleTag simpleTag in LogglyAppConfig.Instance.Tags.Simple)
            {
                config.Tags.SimpleTags.Add(simpleTag);
            }

            foreach (ComplexTagAppConfig complexTagConfig in LogglyAppConfig.Instance.Tags.Complex)
            {
                var assembly = complexTagConfig.Assembly;
                if (string.IsNullOrEmpty(assembly))
                {
                    // Support minimal config with a default when unspecified
                    assembly = "Loggly.Config";
                }
                var complexTag = (ComplexTag)Activator.CreateInstance(assembly, complexTagConfig.Type).Unwrap();
                complexTag.Formatter = complexTagConfig.Formatter;
                config.Tags.ComplexTags.Add(complexTag);
            }

            config.Search = LogglyAppConfig.Instance.Search;

            return config;
        }
    }
}
