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
      public int? NumberOfRows { get; set; }

      public virtual IDictionary<string, object> ToParameters()
      {
         return new Dictionary<string, object>
                {
                   { "q", Query },
                   { "size", NumberOfRows },
                   { "from", From == null ? null : From.Value.ToLogglyDateTime() },
                   { "until", Until == null ? null : Until.Value.ToLogglyDateTime() }
                };
      }
   }

    public class FieldQuery : SearchQuery
    {
        public string FieldName { get; set; }

        public override IDictionary<string, object> ToParameters()
        {
            IDictionary<string, object> parameters = base.ToParameters();
            parameters.Add("fieldname", this.FieldName);

            return parameters;
        }
    }
}