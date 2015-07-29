using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loggly
{
    public interface ILogglyClient
    {
        Task<LogResponse> Log(LogglyEvent logglyEvent);
        Task<LogResponse> Log(IEnumerable<LogglyEvent> logglyEvents);
    }
}