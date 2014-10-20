using System;
using System.Collections.Generic;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogglyClient
   {
      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
       void Log(string messageFormat, params object[] messageArgs);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <remarks>
      /// Callback can be null which will give great performance, at the cost of not knowing if a failure occured.
      /// </remarks>
      void Log(MessageOptions options, string messageFormat, params object[] messageArgs);
      
      void Log<TLogAsJson>(TLogAsJson logObject);
      void Log<TLogAsJson>(MessageOptions options, TLogAsJson logObject);
   }
}