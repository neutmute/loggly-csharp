using System;
using System.Collections.Generic;

namespace Loggly.Config
{
    public enum MessageTransport
    {
        Unspecified =0,
        Http = 1,
        SyslogUdp = 2,
        SyslogSecure= 3
    }
    public interface ITagConfiguration
    {
        List<ISimpleTag> SimpleTags { get; }

        List<ComplexTag> ComplexTags { get; }

        List<string> GetRenderedTags();
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
    public interface ISearchConfiguration
    {
        string Account { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
    public interface ILogglyConfig
    {
        string CustomerToken { get; set; }
        string ApplicationName { get; set; }

        bool ThrowExceptions { get; set; }

        bool IsValid { get;  }

        MessageTransport MessageTransport { get; set; }

        ITagConfiguration Tags { get;  }
        
        ISearchConfiguration Search { get; }
    }
}