using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly.Responses
{
   public class SearchJsonResponse
   {
      private IList<JsonRecord> _results;

      [JsonProperty("numFound")]
      public int TotalRecords { get; set; }

      [JsonProperty("data")]
      public IList<JsonRecord> Results
      {
          get { return _results ?? (_results = new List<JsonRecord>(0)); }
      }
   }

   public class JsonRecord
   {
      public DateTime? Timestamp { get; set; }
      public string InputName { get; set; }

      [JsonProperty("ip")]
      public string IpAddress { get; set; }

      public IDictionary<string, object> Json { get; set; }
   }
}