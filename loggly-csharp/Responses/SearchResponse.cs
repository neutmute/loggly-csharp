using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly.Responses
{
   public class SearchResponse
   {
      private IList<Record> _results;

      [JsonProperty("numFound")]
      public int TotalRecords { get; set; }

      [JsonProperty("data")]
      public IList<Record> Results
      {
         get { return _results ?? (_results = new List<Record>(0)); }
      }
   }

   public class Record
   {
      public DateTime Timestamp { get; set; }      
      public string InputName { get; set; }

      [JsonProperty("ip")]
      public string IpAddress { get; set; }

      public string Text { get; set; }
   }
}