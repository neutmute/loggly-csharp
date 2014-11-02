using System;
using Loggly.Responses;

namespace Loggly
{
    internal interface IMessageTransport
    {
        LogResponse Send(LogglyMessage message);
    }
}