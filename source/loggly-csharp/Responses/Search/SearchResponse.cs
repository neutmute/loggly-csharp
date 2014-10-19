using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loggly.Responses
{
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
            var entryResonse = this.Communicator.Search(new Dictionary<string, object>() {{"rsid", this.RSID.Id}, {"page", page}});
            return entryResonse;
        }
    }
}
