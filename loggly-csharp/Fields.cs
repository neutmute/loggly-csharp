using System;

namespace Loggly
{
   [Flags]
   public enum Fields
   {      
      Timestamp = 1,
      Ip = 2,
      InputName = 4,
      Text = 8,
      All = Timestamp | Ip | InputName | Text
   }
}