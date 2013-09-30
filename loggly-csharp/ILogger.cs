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
      LogResponse LogSync(string message, params string[] tags);
      LogResponse LogSync<TMessage>(TMessage message, params string[] tags);

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
      void Log<TMessage>(TMessage message, params string[] tags);
      void Log<TMessage>(TMessage message, Action<LogResponse> callback, params string[] tags);
   }
}