using System;
using Newtonsoft.Json;

namespace Loggly.Responses
{
    public abstract class EventBase
    {
        [JsonProperty("tags")]
        public string[] Tags { get; set; }
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}