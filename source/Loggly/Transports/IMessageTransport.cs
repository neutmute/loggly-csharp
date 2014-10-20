using System;
using Loggly.Responses;

namespace Loggly
{
    public interface IMessageTransport
    {
        void Send(LogglyMessage message, Action<Response> callback);
    }
}