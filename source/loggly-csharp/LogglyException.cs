using System;

namespace Loggly
{
   public class LogglyException : Exception
   {
      public LogglyException(){}
      public LogglyException(string message) : base(message){}
      public LogglyException(string message, Exception innerException) : base(message, innerException){}      
   }
}