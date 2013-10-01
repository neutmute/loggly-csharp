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
      /// <param name="tags">Tags, which Loggly will apply as meta data to your event</param>
      /// <remarks>
      /// Same as calling LogAsync(message, callback) where callback is null
      /// </remarks>
      void Log(string message, params string[] tags);

      /// <summary>
      /// Asynchronously logs a message
      /// </summary>
      /// <param name="message">The message to log</param>
      /// <param name="callback">The callback to execute</param>
      /// <param name="tags">Tags, which Loggly will apply as meta data to your event</param>
      /// <remarks>
      /// Callback can be null which will give great performance, at the cost of not knowing if a failure occured.
      /// </remarks>
      void Log(string message, Action<LogResponse> callback, params string[] tags);
      void LogJson<TMessage>(TMessage message, params string[] tags);
      void LogJson<TMessage>(TMessage message, Action<LogResponse> callback, params string[] tags);

      void LogJson(string message, params string[] tags);
      void LogJson(string message, Action<LogResponse> callback, params string[] tags);
   }
}