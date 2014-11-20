using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogglyClient
   {
       Task<LogResponse> Log(LogglyEvent logglyEvent);
   }
}