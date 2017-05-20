using System;
using System.Collections.Generic;

namespace Loggly.Config
{
    public interface ITagConfiguration
    {
        List<ITag> Tags { get; }
    }

    public interface IHttpTransport
    {
    }

    public interface ITransportConfiguration
    {
        string EndpointHostname { get; set; }
        int EndpointPort { get; set; }
        LogTransport LogTransport { get; set; }
		bool IsOmitTimestamp { get; set; }
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

        bool IsEnabled { get; set; }

        bool IsValid { get;  }
        
        ITagConfiguration TagConfig { get;  }
        
        ISearchConfiguration Search { get; }
        
        ITransportConfiguration Transport { get; set; }
    }
}