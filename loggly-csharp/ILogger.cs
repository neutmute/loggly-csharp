using System;
using System.Collections.Generic;
using Loggly.Responses;

namespace Loggly
{
   public interface ILogger
   {
      /// <summary>
      /// Synchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>
      LogResponse LogSync(string message);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>      
      /// <remarks>
      /// Same as calling LogAsync(message, callback) where callback is null
      /// </remarks>
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

      void Log(string message, string category);
      void Log(string message, string category, IDictionary<string, object> data);
      void LogInfo(string message);
      void LogInfo(string message, IDictionary<string, object> data);
      void LogVerbose(string message);
      void LogVerbose(string message, IDictionary<string, object> data);
      void LogWarning(string message);
      void LogWarning(string message, IDictionary<string, object> data);
      void LogError(string message, Exception ex);
      void LogError(string message, Exception ex, IDictionary<string, object> data);
   }
}