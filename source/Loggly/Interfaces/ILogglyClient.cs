using System;
using System.Collections.Generic;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogglyClient
   {
      void Log(LogglyEvent logglyEvent);
   }
}