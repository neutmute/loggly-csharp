using Newtonsoft.Json;

namespace Loggly.Responses
{
    public class EventMessage : EventBase
    {
        [JsonProperty("logmsg")]
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }

    public class EventMessage<TMessage> : EventBase
    {
        [JsonProperty("logmsg")]
        public string Json { get; set; }
        [JsonIgnore]
        public TMessage Message { get { return JsonConvert.DeserializeObject<TMessage>(this.Json); } }

        public override string ToString()
        {
            return Json;
        }
    }
}