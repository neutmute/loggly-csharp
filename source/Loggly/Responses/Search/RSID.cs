using Newtonsoft.Json;

namespace Loggly.Responses
{
    [JsonObject]
    public class Rsid
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("date_from")]
        public long From { get; set; }
        [JsonProperty("date_to")]
        public long To { get; set; }
        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id={0}, Status={1}", Id, Status);
        }
    }
}