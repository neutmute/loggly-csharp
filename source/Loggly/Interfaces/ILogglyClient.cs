using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loggly
{
    public interface ILogglyClient
    {
        Task<LogResponse> Log(LogglyEvent logglyEvent, IEnumerable<string> tags = null);
        Task<LogResponse> Log(IEnumerable<LogglyEvent> logglyEvents, IEnumerable<string> tags = null);
    }
}