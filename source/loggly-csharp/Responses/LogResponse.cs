using Newtonsoft.Json;

namespace Loggly.Responses
{
   public class LogResponse
   {
      [JsonProperty("eventstamp")]
      public int TimeStamp { get; set; }
      public bool Success { get; set; }
   }
}