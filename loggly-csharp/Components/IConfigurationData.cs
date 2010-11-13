using System;

namespace Loggly
{
   public interface IConfigurationData
   {
      int Timeout { get; }
      bool Https { get; }
      string ForcedUrl { get; }
   }

   internal class ConfigurationData : IConfigurationData
   {
      private int _timeout = 5000;
      private bool _https = true;

      public int Timeout
      {
         get { return _timeout; }
         set { _timeout = value; }
      }
      public bool Https
      {
         get { return _https; }
         set { _https = value; }
      }

      public string ForcedUrl { get; set; }      
   }
}