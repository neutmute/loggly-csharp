using System;
using System.Diagnostics;
using Loggly.Config;

namespace Loggly
{
   public class LogglyException : Exception
   {
       #region Constructors
        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected LogglyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected LogglyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion

        #region Static Methods
        public static void Throw(string format, params object[] args)
        {
            string message = string.Format(format, args);
            var exception = new LogglyException(message);
            Throw(exception);
        }

        public static void Throw(Exception innerException, string format, params object[] args)
        {
            string message = string.Format(format, args);
            var exception = new LogglyException(message, innerException);
            Throw(exception);
        }

        public static void Throw(Exception innerException)
        {
            var exception = new LogglyException(innerException.Message, innerException);
            Throw(exception);
        }

       private static void Throw(LogglyException exception)
       {
            if (LogglyConfig.Instance.ThrowExceptions)
            {
                throw exception;
            }
            else
            {
                Debug.WriteLine(exception);
            }
       }
        #endregion  
   }
}