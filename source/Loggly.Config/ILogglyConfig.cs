using System;
using System.Collections.Generic;

namespace Loggly.Config
{
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
        string EndpointHostname { get; set; }
        int EndpointPort { get; set; }
        LogTransport LogTransport { get; set; }
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
        
        ITagConfiguration Tags { get;  }
        
        ISearchConfiguration Search { get; }

        ITransportConfiguration Transport { get; }
    }
}