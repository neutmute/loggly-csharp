using System;
using Loggly.Responses;

namespace Loggly
{
    internal interface IMessageTransport
    {
        void Send(LogglyMessage message, Action<Response> callback);
    }
}