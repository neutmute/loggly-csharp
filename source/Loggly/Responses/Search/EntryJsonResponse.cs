using Newtonsoft.Json;

namespace Loggly.Responses
{
    public abstract class EntryJsonResponseBase
    {
        [JsonProperty("total_events")]
        public int TotalEvents { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("timestamp")]
        public int TimeStamp { get; set; }
    }
    public class EntryJsonResponse<TMessage> : EntryJsonResponseBase
    {
        [JsonProperty("events")]
        public EventMessage<TMessage>[] Events { get; set; }
    }

    public class EntryJsonResponse : EntryJsonResponseBase
    {
        [JsonProperty("events")]
        public EventMessage[] Events { get; set; }
    }
}