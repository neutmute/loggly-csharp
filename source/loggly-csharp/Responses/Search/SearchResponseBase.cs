using Newtonsoft.Json;

namespace Loggly.Responses
{
    public abstract class SearchResponseBase
    {
        protected EntryJsonResponseBase FirstEntryResponse { get; set; }

        [JsonIgnore]
        internal Communicator Communicator { get; set; }

        [JsonProperty("rsid")]
        public RSID RSID { get; set; }

        [JsonIgnore]
        public int TotalEvents
        {
            get
            {
                if (this.FirstEntryResponse == null)
                {
                    this.FirstEntryResponse = GetEntryJsonResponse(0);
                }

                return this.FirstEntryResponse.TotalEvents;
            }
        }

        protected abstract EntryJsonResponseBase GetEntryJsonResponse(int page);

    }
}