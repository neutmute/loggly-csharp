using System;
using System.Collections.Generic;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogger
   {
      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>
      void Log(string message);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>
      /// <param name="callback">The callback to execute</param>
      /// <remarks>
      /// Callback can be null which will give great performance, at the cost of not knowing if a failure occured.
      /// </remarks>
      void Log(string message, Action<LogResponse> callback);
      
      void Log<TLogAsJson>(TLogAsJson logObject);
      void Log<TLogAsJson>(TLogAsJson logObject, Action<LogResponse> callback);
      
   }
}