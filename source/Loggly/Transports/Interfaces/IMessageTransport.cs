using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loggly
{
    internal interface IMessageTransport
    {
        Task<LogResponse> Send(IEnumerable<LogglyMessage> message);
    }
}