using System;

namespace Loggly.Responses
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }
}