using System;

namespace Loggly
{
   public static class LogglyExtensions
   {
       public static string ToJsonIso8601(this DateTime date)
       {
           return date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffZ"); 
       }

      public static string ToSyslog(this DateTime date)
      {
          return date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffzzz"); 
      }


      public static string ToSyslog(this DateTimeOffset date)
      {
          return date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffzzz");
      }
   }
}