using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly.Responses
{
   public class FacetResponse
   {
      private IDictionary<string, int> _data;

      [JsonProperty("numFound")]
      public int TotalRecords { get; set; }      
      public int Start { get; set; }      
      public string Gap { get; set; }
      [JsonProperty("gmt_offset")]
      public string GmtOffset { get; set; }     
      public IDictionary<string, int> Data
      {
         get { return _data ?? (_data = new Dictionary<string, int>(0)); }
      }
   }  
}