using System;
using Loggly.Responses;

namespace Loggly
{
    public class EventOptions
    {
        public Action<LogResponse> Callback { get; set; }
    }
}