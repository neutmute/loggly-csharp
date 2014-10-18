using System;
using System.Net;

namespace Loggly
{
   public interface ILogglyConfiguration
   {
      ILogglyConfiguration WithTimeout(int timeout);
      ILogglyConfiguration WithTimeout(TimeSpan timeout);
      ILogglyConfiguration DontUseHttps();
      ILogglyConfiguration AuthenticateWith(string username, string password);

      /// <summary>
      /// Used by the test library
      /// </summary>      
      ILogglyConfiguration ForceUrlTo(string url);
   }

   public class LogglyConfiguration : ILogglyConfiguration
   {
      private static ConfigurationData _data = new ConfigurationData();
      private static readonly LogglyConfiguration _configuration = new LogglyConfiguration();

      protected LogglyConfiguration()
      {
      }

      public static IConfigurationData Data
      {
         get { return _data; }
      }

      public ILogglyConfiguration WithTimeout(int timeout)
      {
         _data.Timeout = timeout;
         return this;
      }

      public ILogglyConfiguration WithTimeout(TimeSpan timeout)
      {
         return WithTimeout((int) timeout.TotalMilliseconds);
      }

      public ILogglyConfiguration DontUseHttps()
      {
         _data.Https = false;
         return this;
      }

      public ILogglyConfiguration AuthenticateWith(string username, string password)
      {
         _data.Credentials = new NetworkCredential(username, password);
         return this;
      }

      /// <summary>
      /// Used by the test library
      /// </summary>
      public ILogglyConfiguration ForceUrlTo(string url)
      {
         _data.ForcedUrl = url;
         return this;
      }

      public static void Configure(Action<ILogglyConfiguration> action)
      {
         action(_configuration);
      }

      /// <summary>
      /// Used by the test library
      /// </summary>
      public static void ResetToDefaults()
      {
         _data = new ConfigurationData();
      }
   }
}