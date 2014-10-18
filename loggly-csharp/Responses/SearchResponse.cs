using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    [JsonObject]
    public class SearchResponse : SearchResponseBase, IEnumerable<EventMessage>
    {

        public IEnumerator<EventMessage> GetEnumerator()
        {
            int page = 0;
            int returnedEntryCount = 0;
            var entryResonse = this.FirstEntryResponse ?? GetEntryJsonResponse(page);

            while (true)
            {
                foreach (EventMessage eventMessage in (entryResonse as EntryJsonResponse).Events)
                {
                    returnedEntryCount++;
                    yield return eventMessage;
                }

                if (returnedEntryCount >= entryResonse.TotalEvents)
                    yield break;

                page++;
                entryResonse = GetEntryJsonResponse(page);
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override EntryJsonResponseBase GetEntryJsonResponse(int page)
        {
            var entryResonse = this.Communicator.GetPayload<EntryJsonResponse>(
                "apiv2/events",
                new Dictionary<string, object>() {{"rsid", this.RSID.Id}, {"page", page}});
            return entryResonse;
        }
    }

    [JsonObject]
    public class SearchResponse<TMessage> : SearchResponseBase, IEnumerable<EventMessage<TMessage>>
    {
        public IEnumerator<EventMessage<TMessage>> GetEnumerator()
        {
            int page = 0;
            int returnedEntryCount = 0;
            var entryResonse = FirstEntryResponse ?? GetEntryJsonResponse(page);

            while (true)
            {
                foreach (EventMessage<TMessage> eventMessage in (entryResonse as EntryJsonResponse<TMessage>).Events)
                {
                    returnedEntryCount++;
                    yield return eventMessage;
                }

                if (returnedEntryCount >= entryResonse.TotalEvents)
                    yield break;

                page++;
                entryResonse = GetEntryJsonResponse(page);
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override EntryJsonResponseBase GetEntryJsonResponse(int page)
        {
            EntryJsonResponse<TMessage> entryResonse = this.Communicator.GetPayload<EntryJsonResponse<TMessage>>(
                "apiv2/events",
                new Dictionary<string, object>() { { "rsid", this.RSID.Id }, { "page", page } });
            return entryResonse;
        }
    }

    public abstract class EntryJsonResponseBase
    {
        [JsonProperty("total_events")]
        public int TotalEvents { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("timestamp")]
        public int TimeStamp { get; set; }
    }

    public class EntryJsonResponse : EntryJsonResponseBase
    {
        [JsonProperty("events")]
        public EventMessage[] Events { get; set; }
    }

    public class EntryJsonResponse<TMessage> : EntryJsonResponseBase
    {
        [JsonProperty("events")]
        public EventMessage<TMessage>[] Events { get; set; }
    }

    public abstract class EventBase
    {
        [JsonProperty("tags")]
        public string[] Tags { get; set; }
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class EventMessage : EventBase
    {
        [JsonProperty("logmsg")]
        public string Message { get; set; }
    }

    public class EventMessage<TMessage> : EventBase
    {
        [JsonProperty("logmsg")]
        public string Json { get; set; }
        [JsonIgnore]
        public TMessage Message { get { return JsonConvert.DeserializeObject<TMessage>(this.Json); } }
    }

    [JsonObject]
    public class RSID
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
    }

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
