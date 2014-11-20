using System;
using System.Collections.Generic;
using System.Text;

namespace Loggly
{
   public class SearchQuery
   {
      public string Query { get; set; }
      public DateTime? From { get; set; }
      public DateTime? Until { get; set; }
      public int? Size { get; set; }

      public virtual IDictionary<string, object> ToParameters()
      {
         return new Dictionary<string, object>
                {
                   { "q", Query },
                   { "size", Size },
                   { "from", From == null ? null : From.Value.ToJsonIso8601() },
                   { "until", Until == null ? null : Until.Value.ToJsonIso8601() }
                };
      }
   }
}