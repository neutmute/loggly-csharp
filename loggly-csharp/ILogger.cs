using System;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogger
   {
      /// <summary>
      /// Synchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>     
      LogResponse Log(string message);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>      
      /// <remarks>
      /// Same as calling LogAsync(message, callback) where callback is null
      /// </remarks>
      void LogAsync(string message);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>
      /// <param name="callback">The callback to execute</param>
      /// <remarks>
      /// Callback can be null which will give great performance, at the cost of not knowing if a failure occured.
      /// </remarks>
      void LogAsync(string message, Action<LogResponse> callback);
   }
}