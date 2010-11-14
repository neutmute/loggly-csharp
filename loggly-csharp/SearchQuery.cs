using System;
using System.Collections.Generic;

namespace Loggly
{
   public class SearchQuery
   {
      public string Query { get; set; }
      public DateTime? From { get; set; }
      public DateTime? Until { get; set; }
      public int? StartingAt { get; set; }
      public int? NumberOfRows { get; set; }

      public IDictionary<string, object> ToParameters()
      {
         return new Dictionary<string, object>
                {
                   { "q", Query },
                   { "rows", NumberOfRows },
                   { "start", StartingAt },
                   { "from", From == null ? null : From.Value.ToLogglyDateTime() },
                   { "until", Until == null ? null : Until.Value.ToLogglyDateTime() },
                };
      }
   }
}