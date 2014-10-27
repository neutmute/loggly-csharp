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
            var eventQuery = new EventQuery { Rsid = this.Rsid.Id, Page = page };
            var entryResonse = this.Transport.Search(eventQuery);
            return entryResonse;
        }
    }
}