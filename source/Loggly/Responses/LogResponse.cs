using Newtonsoft.Json;

namespace Loggly.Responses
{
   public class LogResponse
   {
      [JsonProperty("eventstamp")]
      public int TimeStamp { get; set; }
      public bool Success { get; set; }

      public string Message { get; set; }

      public override string ToString()
      {
          return string.Format("Success={0}, Message={1}", Success, Message);
      }
   }
}