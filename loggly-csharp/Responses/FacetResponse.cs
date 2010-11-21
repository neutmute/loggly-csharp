//using System.Collections.Generic;
//using Newtonsoft.Json;
//
//namespace Loggly.Responses
//{
//   public class FacetResponse
//   {
//      private IDictionary<string, int> _data;
//
//      [JsonProperty("numFound")]
//      public int TotalRecords { get; set; }
//      [JsonProperty("numFound")]
//      public int Start { get; set; }
//      [JsonProperty("numFound")]
//      public string Gap { get; set; }
//      [JsonProperty("gmt_offset")]
//      public string GmtOffset { get; set; }
//      public Context Context { get; set; }      
//      public IDictionary<string, int> Data
//      {
//         get { return _data ?? (_data = new Dictionary<string, int>(0)); }
//      }
//   }
//
//   public class Context
//   {
//      public string From { get; set; }
//      public string Until { get; set; }
//      public int Start { get; set; }
//      public string Query { get; set; }
//      public string Order { get; set; }
//   }   
//}