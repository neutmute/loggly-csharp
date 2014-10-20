using System.Collections.Generic;

namespace Loggly.Config
{
    public interface ITagConfiguration
    {
        List<ISimpleTag> SimpleTags { get; }

        List<ComplexTag> ComplexTags { get; }

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
        ITransportConfiguration Transport { get;  }
        ISearchConfiguration Search { get; }
    }
}