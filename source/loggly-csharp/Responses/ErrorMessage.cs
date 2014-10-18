using System;

namespace Loggly.Responses
{
    public class ErrorMessage
    {
        public string Error { get; set; }
        public string Info { get; set; }
        public string Maintenance { get; set; }
        public Exception InnerException { get; set; }
    }
}