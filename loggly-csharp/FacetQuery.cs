using System;
using System.Collections.Generic;

namespace Loggly
{
   public class FacetQuery
   {
      public string Query { get; set; }
      public DateTime? From { get; set; }
      public DateTime? Until { get; set; }
      public Order? OrderBy { get; set; }
      public int? NumberOfBuckets { get; set; }
      public string Gap { get; set; }
      public Fields? FieldsToFacet { get; set; }

      public IDictionary<string, object> ToParameters()
      {
         return new Dictionary<string, object>
                {
                   { "q", Query },
                   { "buckets", NumberOfBuckets == null ? null : (int?)NumberOfBuckets.Value },
                   { "order", SerializeOrder(OrderBy) },
                   { "from", From == null ? null : From.Value.ToLogglyDateTime() },
                   { "until", Until == null ? null : Until.Value.ToLogglyDateTime() },
                   { "gap", Gap},
                   { "facetby", SerializeFields(FieldsToFacet)},
                };
      }

      private static string SerializeOrder(Order? orderBy)
      {
         if (orderBy == Order.Ascending) { return "asc"; }
         if (orderBy == Order.Descending) { return "desc"; }
         return null;
      }

      private static string SerializeFields(Fields? fieldsToFacet)
      {         
         if (fieldsToFacet  == Fields.Ip) { return "ip"; }
         if (fieldsToFacet  == Fields.InputName) { return "inputname"; }
         if (fieldsToFacet == Fields.Text) { return "text"; }
         return null;
      } 
   }
}