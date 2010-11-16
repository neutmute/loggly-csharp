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
      public int? StartingAt { get; set; }
      public int? NumberOfRows { get; set; }
      public Fields? FieldsToSelect { get; set; }

      public IDictionary<string, object> ToParameters()
      {
         return new Dictionary<string, object>
                {
                   { "q", Query },
                   { "rows", NumberOfRows },
                   { "start", StartingAt },
                   { "from", From == null ? null : From.Value.ToLogglyDateTime() },
                   { "until", Until == null ? null : Until.Value.ToLogglyDateTime() },
                   { "fields", SerializeFields(FieldsToSelect)},
                };
      }

      private static string SerializeFields(Fields? fieldsToSelect)
      {
         if (fieldsToSelect == null || fieldsToSelect == Fields.All) { return null; }

         var sb = new StringBuilder();
         if ((fieldsToSelect & Fields.Ip) == Fields.Ip) { sb.Append("ip,"); }
         if ((fieldsToSelect & Fields.InputName) == Fields.InputName) { sb.Append("inputname,"); }
         if ((fieldsToSelect & Fields.Text) == Fields.Text) { sb.Append("text,"); }
         if ((fieldsToSelect & Fields.Timestamp) == Fields.Timestamp) { sb.Append("timestamp,"); }

         return sb.Length > 0 ? sb.Remove(sb.Length - 1, 1).ToString() : null;
      }
   }
}