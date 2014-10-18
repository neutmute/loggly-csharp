using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly.Responses
{
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
}