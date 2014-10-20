using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
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
            var eventQuery = new EventQuery {Rsid = this.RSID.Id, Page = page};
            var entryResonse = this.Transport.Search(eventQuery);
            return entryResonse;
        }
    }
}
