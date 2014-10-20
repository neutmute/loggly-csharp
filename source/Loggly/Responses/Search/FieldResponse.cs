using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loggly.Responses
{
    [JsonObject]
    public class FieldResponse
    {

        public Field[] Fields { get; private set; }

        public FieldResponse(JObject json, string fieldName)
        {
            this.Fields = json[fieldName]
                .Take(json["unique_field_count"].ToObject<int>())
                .Select(field => field.ToObject<Field>())
                .ToArray();
        }
    }
    public class Field
    {
        [JsonProperty("term")]
        public string Term { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }

    }
}